using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using Entities.Factories;
using Entities.Inventory;
using Entities.LocationActors;
using Entities.RNG;
using Serilog;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    public class SystemAndProbeSteps
    {
        private readonly ScenarioContextState _state;

        public SystemAndProbeSteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I set the config to")]
        public void GivenISetTheConfigTo(string multilineText)
        {
            _state.Config = multilineText;
        }

        [When(@"I send shutdown to the actor ""(.*)""")]
        public void WhenIShutdownToTheActor(string actorName)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"I create a test actor system using config")]
        [Given(@"I create a test actor system using config ""(.*)""")]
        public void GivenICreateATestActorSystem(string config)
        {
            _state.Config = config;
            GivenICreateATestActorSystem();
        }

        [Given(@"I create a test actor system")]
        public void GivenICreateATestActorSystem()
        {
            _state.TestKit = new TestKit(_state.Config, "testActorSystem");
            RootLevelActors.SetActorSystem(_state.TestKit.Sys);
            RootLevelActors.SetupRootLevelActors(null);
            AddRootLevelActorsToScenarioState();
        }

        private void AddRootLevelActorsToScenarioState()
        {
            _state.Actors.Add(ResourceManager.Name, RootLevelActors.ResourceManagerActorRef);
            _state.Actors.Add(HeartBeatActor.Name,RootLevelActors.HeartBeatActorRef);
            _state.Actors.Add(FactoryCoordinatorActor.Name, RootLevelActors.FactoryCoordinatorActorRef);
            _state.Actors.Add(InventoryTypeCoordinator.Name, RootLevelActors.InventoryTypeCoordinatorActorRef);
            _state.Actors.Add(CenterOfMassManagerActor.Name, RootLevelActors.CenterOfMassManagerActorRef);
            _state.Actors.Add(RandomIntActor.Name, RootLevelActors.GeneratorActors.RandomIntActorRef);
            _state.Actors.Add(RandomDoubleActor.Name, RootLevelActors.GeneratorActors.RandomDoubleActorRef);
        }

        [Given(@"I create a TestProbe called ""(.*)""")]
        public void GivenICreateATestProbeCalled(string name)
        {
            var testProbe = _state.TestKit.CreateTestProbe(name);
            _state.TestProbes.Add(name, testProbe);
            _state.Actors.Add(name, testProbe);
        }

        [When(@"I restart the actor system")]
        public void WhenIRestartTheActorSystem()
        {
            _state.TestKit.Shutdown();
            GivenICreateATestActorSystem();
        }
    }
}
