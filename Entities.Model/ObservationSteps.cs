using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Observation;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    public class ObservationSteps
    {
        private readonly ScenarioContextState _state;

        public ObservationSteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I send an observe message to actor ""(.*)"" from actor ""(.*)""")]
        public void GivenISendAnObserveMessageToActorFromActor(string target, string source)
        {
            var t = _state.Actors[target];
            var s = _state.Actors[source];

            t.Tell(new Observe(), s);
        }

        [Then(@"I expect the TestProbe ""(.*)"" to recieve an event message")]
        public void ThenActorCanExpectAnEventMessage(string actor)
        {
            var tp = _state.TestProbes[actor];
            tp.ExpectMsg<EventObserved>();
        }
    }
}
