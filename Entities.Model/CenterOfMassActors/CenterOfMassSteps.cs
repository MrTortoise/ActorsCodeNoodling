using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util.Internal;
using Entities.LocationActors;
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

        [Given(@"I have created the following Moon Type called ""(.*)""")]
        public void GivenIHaveCreatedTheFollowingMoonTypeCalled(string moonTypeName, Table table)
        {
            var resourceComposition = GetResourceComposition(table);

            var moonType = new Moon.MoonType(moonTypeName, resourceComposition);
            _state.MoonTypes.Add(moonType.Name, moonType);
        }

        private Dictionary<IResource, double> GetResourceComposition(Table table)
        {
            var resources =
                _state.ResourceManager.Ask<ResourceManager.GetResourceResult>(new ResourceManager.GetResource(null));

            resources.Wait();

            var resourceComposition = new Dictionary<IResource, double>();

            foreach (var tableRow in table.Rows)
            {
                var resourceName = tableRow["ResourceName"];
                double val = double.Parse(tableRow["Value"]);
                var resource = resources.Result.Values.Single(i => i.Name == resourceName);
                resourceComposition.Add(resource, val);
            }
            return resourceComposition;
        }

        [Given(@"I have created the following moons")]
        public void GivenIHaveCreatedTheFollowingMoons(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                var celestialBody = GetCelestialBody(tableRow);
                var moonType = _state.MoonTypes[tableRow["moonType"]];
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

        [Given(@"I have created the following Planet Type called ""(.*)""")]
        public void GivenIHaveCreatedTheFollowingPlanetTypeCalled(string name, Table table)
        {
            var resourceComposition = GetResourceComposition(table);

            var planetType = new Planet.PlanetType(name, resourceComposition);
            _state.PlanetTypes.Add(planetType.Name, planetType);
        }

        [Given(@"I have created the following planets")]
        public void GivenIHaveCreatedTheFollowingPlanets(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                var celestialBody = GetCelestialBody(tableRow);
                var planetType = _state.PlanetTypes[tableRow["planetType"]];

                var moonString = tableRow["moons"];
                var strings = ExtractStringsFromCsv(moonString);

                var moons = strings.Select(i => _state.Moons[i]);

                var planet = new Planet(planetType, celestialBody, moons.ToArray());
                _state.Planets.Add(planet.BodyData.Name, planet);
            }
        }

        private static string[] ExtractStringsFromCsv(string input)
        {
            var strings = input.Split(',');
            strings = strings.Select(i => i.Replace("\"", "")).ToArray();
            return strings;
        }

        [Given(@"I have created the following Star Type called ""(.*)""")]
        public void GivenIHaveCreatedTheFollowingStarTypeCalled(string name, Table table)
        {
            var fuelRate = double.Parse(table.Rows[0]["fuelType"]);

            var starType = new Star.StarType(name, fuelRate);
            _state.StarTypes.Add(starType.Name, starType);
        }

        [Given(@"I have created the following stars")]
        public void GivenIHaveCreatedTheFollowingStars(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                var celestialBody = GetCelestialBody(tableRow);
                var starType = _state.StarTypes[tableRow["starType"]];

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
            }
        }

    }
}
