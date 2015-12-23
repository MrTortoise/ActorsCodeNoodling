using System;
using System.Threading;
using Akka.Actor;
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

            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            heartBeatActor.Tell(new HeartBeatActor.ConfigureUpdate(updatePeriod, factoryUpdatePeriod));
            Thread.Sleep(10);
        }
        
        [When(@"I tell the heartbeat actor to start")]
        public void WhenITellTheHeartbeatActorToStart()
        {
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            heartBeatActor.Tell(new HeartBeatActor.Start());
        }

        [Given(@"I register actor ""(.*)"" with the HeartBeat actor")]
        public void GivenIRegirsterActorWithTheHeartBeatActor(string actorName)
        {
            var heartBeatActor = _state.Actors[HeartBeatActor.Name];
            var actor = _state.Actors[actorName];
            heartBeatActor.Tell(new HeartBeatActor.Register(HeartBeatActor.UpdateType.Factory,actor),actor);
            Thread.Sleep(10);
        }

        [Then(@"I expect the actor ""(.*)"" to recieve the Start message")]
        public void ThenIExpectTheActorToRecieveTheStartMessage(string actorName)
        {
            var actor = _state.TestProbes[actorName];
            var msg = actor.ExpectMsg<HeartBeatActor.Started>();
        }
    }
}
