using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Entities.RNG;

namespace Entities.UniverseGenerator
{
    public class GenerateCdfToDistributionJob : ReceiveActor, IWithUnboundedStash
    {
        private readonly IActorRef _randomActor;
        private ISingleVariableFunction<int,double> _function;
        private IActorRef _sender;
        private string _distributionName;

        /// <summary>
        /// Gets or sets the stash. This will be automatically populated by the framework AFTER the constructor has been run.
        ///             Implement this as an auto property.
        /// </summary>
        /// <value>
        /// The stash.
        /// </value>
        public IStash Stash { get; set; }

        public GenerateCdfToDistributionJob(IActorRef randomActor)
        {
            _randomActor = randomActor;
            Become(Waiting);
        }

        private void Waiting()
        {
            Receive<Start>(msg =>
            {
                _function = msg.Function;
                _sender = Sender;
                _distributionName = msg.DistributionName;
                _randomActor.Tell(new RandomDoubleActor.NextRandom(msg.NoResults));
                BecomeStacked(Working);
            });
        }

        private void Working()
        {
            Receive<RandomDoubleActor.RandomResult>(msg =>
            {
                int[] output = new int[msg.Numbers.Length];
                for (var i = 0; i < msg.Numbers.Length; i++)
                {
                    output[i] = _function.F(msg.Numbers[i]);
                }
                _sender.Tell(new Completed(output,_distributionName));
                Stash.UnstashAll();
                UnbecomeStacked();
            });

            ReceiveAny(o => Stash.Stash());
        }

        public class Completed
        {
            public int[] Result { get; }
            public string DistributionName { get; }

            public Completed(int[] result, string distributionName)
            {
                Result = result;
                DistributionName = distributionName;
            }
        }


        public static Props CreateProps(IActorRef randomActor)
        {
            return Props.Create(()=>new GenerateCdfToDistributionJob(randomActor));
        }

        public class Start
        {
            public ISingleVariableFunction<int, double> Function { get;  }
            public int NoResults { get;  }
            public string DistributionName { get;  }

            public Start(ISingleVariableFunction<int, double> function, int noResults, string distributionName)
            {
                Function = function;
                NoResults = noResults;
                DistributionName = distributionName;
            }
        }
    }
}
