using System;
using Akka.Util.Internal;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.AutoFackery
{
    [Binding()]
    [Scope(Tag = "Autofackery")]
    public class AutoFacSteps
    {
        private AutoFackeryState _state;
        private IIterator _iterator;

        [BeforeFeature()]
        public static void BeforeFeature()
        {
            SLBuilder.Initialise();
        }

        [BeforeScenario()]
        public void BeforeScenario()
        {
            _state = SLBuilder.State;
        }

        [Given(@"I configure, build and scope with the per feature configuration")]
        public void GivenICreateAContainerInThePerFeatureConfiguration()
        {
            var module = new PerFeatureModule();
            ConfigureStateStartScope(new IModule[] { module });
        }

        [Given(@"I resolve an instance of the iterator and store it in the context")]
        public void GivenIResolveAnInstanceOfTheIteratorAndStoreItInTheContext()
        {
            var iterator = _state.Scope.Resolve<IIterator>();
            _iterator = iterator;
        }
        
        [When(@"I iterate the iterators value")]
        public void WhenIIterateTheIteratorsValue()
        {
           _iterator.Iterate();
        }
        
        [Then(@"I expect the iterators value to be (.*)")]
        public void ThenIExpectTheIteratorsValueToBe(int value)
        {
            Assert.AreEqual(value, _iterator.Value);
        }

        [Given(@"I configure, build and scope with the per scenario configuration")]
        public void GivenICreateAContainerInThePerScenarioConfiguration()
        {
            var perScenarioModule = new PerScenarioModule();
            ConfigureStateStartScope(new IModule[] {perScenarioModule});
        }

        private void ConfigureStateStartScope(IModule[] modules)
        {
            if (modules == null) throw new ArgumentNullException(nameof(modules));

            _state.Builder = new ContainerBuilder();
            modules.ForEach(_=>_state.Builder.RegisterModule(_));
            _state.Container = _state.Builder.Build();
            _state.Scope = _state.Container.BeginLifetimeScope();
        }
    }

    // ReSharper disable once InconsistentNaming
    public static class SLBuilder
    {
        private static AutoFackeryState _state;

        public static AutoFackeryState State
        {
            get
            {
                if (_state == null)
                {
                    Initialise();
                }
                return _state;
            }
        }

        public static void Initialise(ContainerBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }
            _state = new AutoFackeryState(builder);
        }
    }
}
