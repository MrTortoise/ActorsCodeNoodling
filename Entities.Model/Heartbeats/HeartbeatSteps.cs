using System;
using System.Threading;
using Akka.Actor;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.Heartbeats
{
    [Binding]
    public class HeartbeatSteps
    {
        private readonly ScenarioContextState _state;

        public HeartbeatSteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I have created the HeartBeat actor")]
        public void GivenIHaveCreatedAHeartBeatActor()
        {
            CreateHeartbeatActor(_state);
        }

        public static void CreateHeartbeatActor(ScenarioContextState scenarioContextState)
        {
            var heartBeatActor = scenarioContextState.TestKit.Sys.ActorOf(HeartBeatActor.CreateProps(), HeartBeatActor.Name);
            scenarioContextState.Actors.Add(HeartBeatActor.Name, heartBeatActor);
        }

        [Given(@"I have configured the heartBeat actor to update with the following configuration")]
        public void GivenIHaveConfiguredItToUpdateWithTheFollowingConfiguration(Table table)
        {
            //| updatePeriod | factoryUpdatePeriod |
            //| 100 | 1000 |

            var tableRow = table.Rows[0];

            int updatePeriodMilliseconds = int.Parse(tableRow["updatePeriod"]);
            int factoryUpdatePeriodMilliseconds = int.Parse(tableRow["factoryUpdatePeriod"]);

            TimeSpan updatePeriod = TimeSpan.FromMilliseconds(updatePeriodMilliseconds);
            TimeSpan factoryUpdatePeriod = TimeSpan.FromMilliseconds(factoryUpdatePeriodMilliseconds);

            if (!_state.Actors.ContainsKey(HeartBeatActor.Name))
            {
                CreateHeartbeatActor(_state);
            }

            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            heartBeatActor.Tell(new HeartBeatActor.ConfigureUpdate(updatePeriod, factoryUpdatePeriod));
            Thread.Sleep(10);
        }

        [Given(@"I tell the heartbeat actor to start")]
        [When(@"I tell the heartbeat actor to start")]
        public void WhenITellTheHeartbeatActorToStart()
        {
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];

            var heartBeatActorStartedWatcher = _state.TestKit.CreateTestProbe("heartBeatActorStartWatch");
            heartBeatActor.Tell(new HeartBeatActor.Register(HeartBeatActor.UpdateType.Tick, heartBeatActorStartedWatcher));
            heartBeatActor.Tell(new HeartBeatActor.Start());

            var msg = heartBeatActorStartedWatcher.ExpectMsg<HeartBeatActor.Started>();

            Assert.IsNotNull(msg);
        }

        [Given(@"I register actor ""(.*)"" with the HeartBeat actor as type ""(.*)""")]
        public void GivenIRegisterActorWithTheHeartBeatActorAsType(string actorName, string updateTypeString)
        {
            HeartBeatActor.UpdateType updateType = (HeartBeatActor.UpdateType)Enum.Parse(typeof (HeartBeatActor.UpdateType), updateTypeString);
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            var actor = _state.Actors[actorName];

            var registrationWatcher = _state.TestKit.CreateTestProbe("HeartBeatRegistrationWatcher");

            heartBeatActor.Tell(new HeartBeatActor.Register(updateType, actor), registrationWatcher);
            registrationWatcher.ExpectMsg<HeartBeatActor.Registered>(registered => ReferenceEquals(registered.Registree.Actor, actor) && registered.Registree.UpdateType == updateType);
        }


        [Then(@"I expect the actor ""(.*)"" to recieve the Start message")]
        public void ThenIExpectTheActorToRecieveTheStartMessage(string actorName)
        {
            var actor = _state.TestProbes[actorName];
            var msg = actor.ExpectMsg<HeartBeatActor.Started>();
            Assert.IsNotNull(msg);
        }

        [When(@"I wait for (.*) FactoryUpdate time periods")]
        public void WhenIWaitForFactoryUpdateTimePeriods(int timePeriods)
        {
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            var configQuery = heartBeatActor.Ask<HeartBeatActor.ConfigurationResult>(new HeartBeatActor.QueryConfiguration());
            configQuery.Wait();

            var period = configQuery.Result.State.FactoryUpdatePeriod;
            TimeSpan result = period;
            for (int i = 0; i < timePeriods; i++)
            {
                result = result + period;
            }

            var waiterAwaiter = _state.TestKit.CreateTestProbe("waiterTestProbe");
            var waiter = _state.TestKit.Sys.ActorOf(WaitActor.CreateProps());
            waiter.Tell(new WaitActor.Wait((int)result.TotalMilliseconds), waiterAwaiter);
            var msg = waiterAwaiter.ExpectMsg<WaitActor.WaitComplete>(result + TimeSpan.FromSeconds(3));
            Assert.IsNotNull(msg);
        }

        [When(@"I wait for (.*) TickUpdate time periods")]
        public void WhenIWaitForTickUpdateTimePeriods(int timePeriods)
        {
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            var configQuery = heartBeatActor.Ask<HeartBeatActor.ConfigurationResult>(new HeartBeatActor.QueryConfiguration());
            configQuery.Wait();

            var period = configQuery.Result.State.UpdatePeriod;
            TimeSpan result = period;
            for (int i = 0; i < timePeriods; i++)
            {
                result = result + period;
            }

            var waiterAwaiter = _state.TestKit.CreateTestProbe("waiterTestProbe");
            var waiter = _state.TestKit.Sys.ActorOf(WaitActor.CreateProps());
            waiter.Tell(new WaitActor.Wait((int) result.TotalMilliseconds), waiterAwaiter);
            var msg = waiterAwaiter.ExpectMsg<WaitActor.WaitComplete>(result + TimeSpan.FromSeconds(3));
            Assert.IsNotNull(msg);
        }

        [Then(@"I expect TestProbe ""(.*)"" to receive at least (.*) TickUpdate messages")]
        public void ThenIExpectTestProbeToReceiveAtLeastTickUpdateMessages(string testProbe, int noMessages)
        {
            Assert.Contains(testProbe,_state.TestProbes.Keys);
            var tp = _state.TestProbes[testProbe];

            for (int i = 0; i < noMessages; i++)
            {
                var msg = tp.ExpectMsg<HeartBeatActor.Tick>();
                Assert.IsNotNull(msg);
            }
        }



        public class WaitActor : ReceiveActor
        {
            public static Props CreateProps()
            {
                return Props.Create(() => new WaitActor());
            }

            public WaitActor()
            {
                Receive<Wait>(msg =>
                {
                    var sender = Sender;
                    Thread.Sleep(msg.TimeSpan);
                    sender.Tell(new WaitComplete());
                });
            }

            public class WaitComplete
            {
            }

            public class Wait
            {
                public Wait(int timeSpan)
                {
                    TimeSpan = timeSpan;
                }

                public int TimeSpan { get; private set; }
            }
        }
    }
}
