using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Entities.Factories;
using Entities.LocationActors;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.FactoryTests
{
    [Binding]
    public class FactoryUpdateSteps
    {
        private readonly ScenarioContextState _state;

        public FactoryUpdateSteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I set the FactoryCoordinator time period to ""(.*)""")]
        public void GivenISetTheFactoryCoordinatorTimePeriodTo(string timespan)
        {
            var period = TimeSpan.Parse(timespan);
            _state.FactoryCoordinator.Period = period;
        }

        //[Given(@"I have created a FactoryCoordinator actor")]
        //public void CreateFactoryCoordinatorActor()
        //{
        //    GivenIHaveCreatedAFactoryCoordinatorActor(_state);
        //}

        public static void GivenIHaveCreatedAFactoryCoordinatorActor(ScenarioContextState scenarioContextState, IActorRef heartBeatActor)
        {
            var actor = scenarioContextState.TestKit.Sys.ActorOf(FactoryCoordinatorActor.CreateProps(heartBeatActor), FactoryCoordinatorActor.Name);
            scenarioContextState.FactoryCoordinator.Actor = actor;
            scenarioContextState.Actors.Add(actor.Path.Name, actor);
        }

        [Given(@"I have created a Factory Type called ""(.*)"" with the following properties")]
        public void GivenIHaveCreatedAFactoryTypeCalledWithTheFollowingProperties(string factoryTypeName , Table table)
        {
            var factoryType = ExtractFactoryType(factoryTypeName, table);

            _state.FactoryCoordinator.Actor.Tell(factoryType);
            _state.FactoryCoordinator.FactoryTypes.Add(factoryType.Name, factoryType);
        }

        private FactoryType ExtractFactoryType(string factoryTypeName, Table table)
        {
            var inputResourceBuilder = ImmutableDictionary.CreateBuilder<IResource, FactoryType.QuantityPeriod>();
            var outputResourceBuilder = ImmutableDictionary.CreateBuilder<IResource, FactoryType.QuantityPeriod>();

            foreach (var tableRow in table.Rows)
            {
                var resource = _state.GetResourceFromName(tableRow["resource"]);
                var quantity = int.Parse(tableRow["quantity"]);
                int periods = int.Parse(tableRow["periods"]);

                var quantityPeriod = new FactoryType.QuantityPeriod(quantity, periods);

                bool isIn = bool.Parse(tableRow["In"]);
                if (isIn)
                {
                    inputResourceBuilder.Add(resource, quantityPeriod);
                }
                else
                {
                    outputResourceBuilder.Add(resource, quantityPeriod);
                }
            }

            var factoryType = new FactoryType(factoryTypeName,
                inputResourceBuilder.ToImmutable(),
                outputResourceBuilder.ToImmutable());

            return factoryType;
        }




        [When(@"I query the factory types and store result in context as ""(.*)""")]
        public void WhenIQueryTheFactoryTypesAdnSotreResultInContextAs(string contextKey)
        {
            var factoryTypes = _state.FactoryCoordinator.Actor.Ask<FactoryCoordinatorActor.FactoryTypesResult>(new FactoryCoordinatorActor.QueryFactoryTypes());

            factoryTypes.Wait();

            var types = factoryTypes.Result;
            ScenarioContext.Current[contextKey] = types.FactoryTypes;
        }

        [Then(@"I expect the factory type ""(.*)"" with the following properties in context ""(.*)""")]
        public void ThenIExpectTheFactoryTypeWithTheFollowingPropertiesInContext(string typeName, string contextKey, Table table)
        {
            var factoryType = ExtractFactoryType(typeName, table);
            var types = (FactoryType[]) ScenarioContext.Current[contextKey];

            Assert.Contains(factoryType.Name, types.Select(i => i.Name).ToArray());
        }

        [Given(@"I create the following Factories using actor ""(.*)""")]
        [When(@"I create the following Factories using actor ""(.*)""")]
        public void WhenICreateTheFollowingFactoriesUsingActor(string actorName, Table table)
        {
            //| name | factoryType | centerOfMass | celestialBody |
            //| somethingFromNothingFactory | fuckPhysics | Solar System | Other Planet |

            var traderActor = _state.Traders[actorName];
            var createFactoryTp = _state.TestKit.CreateTestProbe("CreateFactoryTestProbe");

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["name"];
                var factoryTypeName = tableRow["factoryType"];
                var comName = tableRow["centerOfMass"];
                var celestialBodyName = tableRow["celestialBody"];
                var inventoryTypeName = tableRow["inventoryType"];

                var factoryType = _state.FactoryCoordinator.FactoryTypes[factoryTypeName];
                var comTask = _state.CenterOfMassManagerActor.Ask<CenterOfMassManagerActor.CenterOfMassQueryResult>(new CenterOfMassManagerActor.QueryCenterOfMasses(comName));
                comTask.Wait();

                var com = comTask.Result.CenterOfMasses[comName];
                var cbTask = com.Ask<CenterOfMassActor.CenterOfMassQueryResult>(new CenterOfMassActor.CenterOfMassStateQuery());
                cbTask.Wait();

                var bodies = cbTask.Result.Planets.Concat(cbTask.Result.Planets.SelectMany(i => i.Satellites));
                var body = bodies.Single(i => i.Name == celestialBodyName);

                var inventoryType = _state.GetInventoryType(inventoryTypeName);

                com.Tell(new CenterOfMassActor.SubscribeFactoryCreated(),createFactoryTp);
                com.Tell(new CenterOfMassActor.CreateFactoryOnBody(name, factoryType, body, inventoryType), traderActor);
                var msg = createFactoryTp.ExpectMsg<FactoryCoordinatorActor.FactoryCreated>();
                Assert.IsNotNull(msg);
            }
        }

        [Then(@"I expect the FactoryCoordinator to contain the following factories")]
        public void ThenIExpectTheFactoryCoordinatorToContainTheFollowingFactories(Table table)
        {
            //| name | factoryType | centerOfMass | celestialBody |
            //| somethingFromNothingFactory | fuckPhysics | Solar System | Other Planet |
            var factoryQueryResult = _state.GetFactories();
            var factoryStates = GetFactoryStates(factoryQueryResult);

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["name"];
                var factoryTypeName = tableRow["factoryType"];
                var celestialBodyName = tableRow["celestialBody"];
                var inventoryType = tableRow["inventoryType"];

                Assert.That(factoryStates.Any(i =>
                    i.Name == name &&
                    i.FactoryType.Name == factoryTypeName &&
                    i.Body.Name == celestialBodyName &&
                    i.Inventory.InventoryType.Name == inventoryType));
            }
        }

        public static Factory.FactoryState[] GetFactoryStates(FactoryCoordinatorActor.FactoryQueryResult factoryQueryResult)
        {
            var tasks = factoryQueryResult.Factories.Select(i => i.Ask<Factory.FactoryState>(new Factory.QueryState())).ToArray();
            Task.WaitAll(tasks);
            var factoryStates = tasks.Select(i => i.Result).ToArray();
            return factoryStates;
        }


        [Then(@"I expect the results of querying the trader ""(.*)"" for its factories to be")]
        public void ThenIExpectTheResultsOfQueryingTheTraderForItsFactoriesToBe(string traderName, Table table)
        {
            var trader = _state.Traders[traderName];

            var queryTask = trader.Ask<Trader.FactoryQueryResult>(new Trader.QueryFactories());
            queryTask.Wait();
            var factoryTasks = queryTask.Result.Factories.Select(i=>i.Ask<Factory.FactoryState>(new Factory.QueryState())).ToArray();
            Task.WaitAll(factoryTasks);
            var factoryStates = factoryTasks.Select(i => i.Result).ToArray();

            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["name"];
                var factoryTypeName = tableRow["factoryType"];
                var celestialBodyName = tableRow["celestialBody"];
                var inventoryType = tableRow["inventoryType"];

                Assert.That(factoryStates.Any(i =>
                    i.Name == name &&
                    i.FactoryType.Name == factoryTypeName &&
                    i.Body.Name == celestialBodyName &&
                    i.Inventory.InventoryType.Name == inventoryType));
            }
        }

        [Then(@"I expect the factory ""(.*)"" to have the following resources")]
        public void ThenIExpectTheFactoryToHaveTheFollowingResources(string factoryName, Table table)
        {
            var resourceComposition = _state.GetResourceComposition(table);

            var factoryQueryResult = _state.GetFactories();
            var factoryStates = GetFactoryStates(factoryQueryResult);

            var factory = factoryStates.SingleOrDefault(i=>i.Name == factoryName);
            Assert.IsNotNull(factory);

            var inventory = factory.Inventory;
            Assert.IsNotEmpty(inventory.Resources);

            foreach (var resource in resourceComposition.Keys)
            {
                Assert.Contains(resource, inventory.Resources.Keys.ToArray());
                Assert.AreEqual(resourceComposition[resource], inventory.Resources[resource]);
            }
        }
    }
}
