using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Entities.Model.Akka.Testkit.Experiments
{
    public class FsmBecomeActor : ReceiveActor
    {
        public FsmActorState State{ get; private set; }

        public FsmBecomeActor()
        {
            State = FsmActorState.Start;
            Start();
        }

        private void Start()
        {
            Receive<string>(s =>
            {
                if (s == "offer")
                {
                    Become(Offered);
                    State = FsmActorState.Offered;
                }
            });
        }

        private void Offered()
        {
            Receive<string>(s =>
            {
                switch (s)
                {
                    case "accept":
                        Become(Accepted);
                        State = FsmActorState.Accepted;
                        break;
                    case "reject":
                        Become(Start);
                        State = FsmActorState.Start;
                        break;
                }
            });
        }

        private void Accepted()
        {
            
        }
    }
}
