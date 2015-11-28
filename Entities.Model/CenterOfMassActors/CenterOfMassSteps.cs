using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using Entities.LocationActors;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model.CenterOfMassActors
{
    [Binding]
    public class CenterOfMassSteps
    {
        private readonly ScenarioContextState _state;

        public CenterOfMassSteps(ScenarioContextState state)
        {
            _state = state;
        }

        [Given(@"I have created the following Material called ""(.*)""")]
        public void GivenIHaveCreatedTheFollowingMaterialCalled(string materialName, Table table)
        {
            var resourceComposition = GetResourceComposition(table);

            var material = new Material(materialName, resourceComposition);
            _state.Materials.Add(material.Name, material);
        }

        private ImmutableDictionary<IResource, double> GetResourceComposition(Table table)
        {
            var resources =
                _state.ResourceManager.Ask<ResourceManager.GetResourceResult>(new ResourceManager.GetResource(null));

            resources.Wait();

            var resourceCompositionBuilder = ImmutableDictionary.CreateBuilder<IResource, double>();

            foreach (var tableRow in table.Rows)
            {
                var resourceName = tableRow["ResourceName"];
                double val = double.Parse(tableRow["Value"]);
                var resource = resources.Result.Values.Single(i => i.Name == resourceName);
                resourceCompositionBuilder.Add(resource, val);
            }
            return resourceCompositionBuilder.ToImmutable();
        }

        [Given(@"I have created the following moons")]
        public void GivenIHaveCreatedTheFollowingMoons(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                var celestialBody = GetCelestialBody(tableRow);
                var moonType = _state.Materials[tableRow["moonType"]];
                var moon = new Moon(moonType, celestialBody);
                _state.Moons.Add(moon.BodyData.Name, moon);
            }
        }

        private static CelestialBody GetCelestialBody(TableRow tableRow)
        {
            var name = tableRow["name"];
            double radius = double.Parse(tableRow["radius"]);
            double orbitDistance = double.Parse(tableRow["orbitDistance"]);
            double orbitalAngularVelocity = double.Parse(tableRow["orbitalAngularVelocity"]);
            double rotatationalAngularVelocity = double.Parse(tableRow["rotatationalAngularVelocity"]);
            double initialOrbitalAngularPositionOffset =
                double.Parse(tableRow["initialOrbitalAngularPositionOffset"]);
            double currentAngularPosition = double.Parse(tableRow["currentAngularPosition"]);

            var celestialBody = new CelestialBody(name, radius, orbitDistance, orbitalAngularVelocity,
                rotatationalAngularVelocity, initialOrbitalAngularPositionOffset, currentAngularPosition);
            return celestialBody;
        }

        [Given(@"I have created the following planets")]
        public void GivenIHaveCreatedTheFollowingPlanets(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                var celestialBody = GetCelestialBody(tableRow);
                var planetType = _state.Materials[tableRow["planetType"]];

                var moonString = tableRow["moons"];
                var strings = ExtractStringsFromCsv(moonString);

                Moon[] moons = null;
                if (!(strings.Length == 1 && string.IsNullOrWhiteSpace(strings[0])))
                {
                    foreach (var s in strings)
                    {
                        Assert.Contains(s, _state.Moons.Keys);
                    }

                    moons = strings.Select(i => _state.Moons[i]).ToArray();
                }

                var planet = new Planet(planetType, celestialBody, moons);
                _state.Planets.Add(planet.BodyData.Name, planet);
            }
        }

        private static string[] ExtractStringsFromCsv(string input)
        {
            var strings = input.Split(',');
            strings = strings.Select(i => i.Replace("\"", "").Trim()).ToArray();
            return strings;
        }

        [Given(@"I have created the following stars")]
        public void GivenIHaveCreatedTheFollowingStars(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                var celestialBody = GetCelestialBody(tableRow);
                var starType = _state.Materials[tableRow["starType"]];

                var star = new Star(starType, celestialBody);
                _state.Stars.Add(star.BodyData.Name, star);
            }
        }

        [Given(@"I create a CenterOfMassManagerActor")]
        public void GivenICreateACenterOfMassManagerActorCalled()
        {
            var comManager = _state.TestKit.Sys.ActorOf(CenterOfMassManagerActor.CreateProps(), CenterOfMassManagerActor.Name);
            _state.CenterOfMassManagerActor = comManager;
            _state.Actors.Add(CenterOfMassManagerActor.Name, comManager);
        }

        [Given(@"I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments")]
        [When(@"I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments")]
        public void WhenISendMessagesOfTypeCreateCenterOfMassToActorCenterOfMassManagerActorWithArguments(Table table)
        {
            var messages = new List<CenterOfMassManagerActor.CreateCenterOfMass>();
            foreach (var tableRow in table.Rows)
            {
                var name = tableRow["name"];
                var starsCsv = tableRow["stars"];
                var planetsCsv = tableRow["planets"];

                var starStrings = ExtractStringsFromCsv(starsCsv);
                var planetStrings = ExtractStringsFromCsv(planetsCsv);

                var stars = starStrings.Select(i => _state.Stars[i]).ToArray();
                var planets = planetStrings.Select(i => _state.Planets[i]).ToArray();

                messages.Add(new CenterOfMassManagerActor.CreateCenterOfMass(name, stars, planets));
            }

            var comManager = _state.CenterOfMassManagerActor;
            foreach (var message in messages)
            {
                comManager.Tell(message);
                Thread.Sleep(10);
            }
           
        }

        [When(@"I get the CenterOfMassActor called ""(.*)"" and store it in the context as ""(.*)""")]
        public void WhenIGetTheCenterOfMassActorCalledAndStoreItInTheContextAs(string comActor, string contextKey)
        {
            var result = _state.CenterOfMassManagerActor.Ask<CenterOfMassManagerActor.CenterOfMassQueryResult>(
                new CenterOfMassManagerActor.QueryCenterOfMasses(comActor));

            result.Wait();
            var actors = result.Result.CenterOfMasses;

            Assert.Contains(comActor, actors.Keys);
            var actor = actors[comActor];

            ScenarioContext.Current.Add(contextKey, actor);
        }

        [Then(@"I Expect the solar system ""(.*)"" to have the following")]
        public void ThenIExpectTheSolarSystemToHaveTheFollowing(string contextKey, Table table)
        {
            IActorRef actor = (IActorRef)ScenarioContext.Current[contextKey];
            var result = actor.Ask<CenterOfMassActor.CenterOfMassQueryResult>(new CenterOfMassActor.CenterOfMassStateQuery());

            result.Wait();

            var r2 = result.Result;

            var stars = r2.Stars;
            var planets = r2.Planets;
            var moons = r2.Planets.SelectMany(i => i.Moons);

            var starNames = table.Rows.Where(i => i["ObjectType"] == "star").Select(i => i["Name"]);
            var planetNames = table.Rows.Where(i => i["ObjectType"] == "planet").Select(i => i["Name"]);
            var moonNames = table.Rows.Where(i => i["ObjectType"] == "moon").Select(i => i["Name"]);

            foreach (var starName in starNames)
            {
                Assert.Contains(starName, stars.Select(i => i.BodyData.Name).ToArray());
            }

            foreach (var planetName in planetNames)
            {
                Assert.Contains(planetName,planets.Select(i=>i.BodyData.Name).ToArray());
            }

            foreach (var moonName in moonNames)
            {
                Assert.Contains(moonName, moons.Select(i => i.BodyData.Name).ToArray());
            }

        }
    }
}
