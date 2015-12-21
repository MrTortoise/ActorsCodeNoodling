using System;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    public class TimeProviderSteps
    {
        private readonly ScenarioContextState _state;

        public TimeProviderSteps(ScenarioContextState state)
        {
            _state = state;
        }

        public static void CreateTimeProviderCrap(ScenarioContextState scenarioContextState)
        {
            var timeProvider = new TestTimeProvider(DateTime.Now);
            scenarioContextState.TimeProducer = timeProvider;
        }
    }
}