using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using Serilog;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    public class SystemAndProbsSteps
    {
        private readonly ScenarioContextState _state;

        public SystemAndProbsSteps(ScenarioContextState state)
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



        [Given(@"I create a test actor system")]
        public void GivenICreateATestActorSystem()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .MinimumLevel.Debug()
                .CreateLogger();
            Serilog.Log.Logger = logger;

            _state.TestKit = new TestKit(_state.Config, "testActorSystem");
        }

        [Given(@"I create a test actor system using config")]
        [Given(@"I create a test actor system using config ""(.*)""")]
        public void GivenICreateATestActorSystem(string config)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .MinimumLevel.Debug()
                .CreateLogger();
            Serilog.Log.Logger = logger;

            _state.Config = config;
            SetupSystem(_state.Config);
        }

        private void SetupSystem(string config)
        {
            _state.TestKit = new TestKit(config, "testActorSystem");
        }

        [Given(@"I create a TestProbe called ""(.*)""")]
        public void GivenICreateATestProbeCalled(string name)
        {
            var testProbe = _state.TestKit.CreateTestProbe(name);
            _state.TestProbes.Add(name, testProbe);
        }

        [When(@"I restart the actor system")]
        public void WhenIRestartTheActorSystem()
        {
            _state.TestKit.Shutdown();
            SetupSystem(_state.Config);
        }

    }
}
