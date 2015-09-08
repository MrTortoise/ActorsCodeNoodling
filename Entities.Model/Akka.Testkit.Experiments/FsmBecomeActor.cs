using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Receive<string>(s => s == "accept", s =>
            {
                Become(Accepted);
                State = FsmActorState.Accepted;
            });
            Receive<string>(s => s == "reject", s =>
            {
                Become(Start);
                State = FsmActorState.Start;
            });
        }

        private void Accepted()
        {
            
        }
    }
}
