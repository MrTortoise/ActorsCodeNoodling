﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit;
using Entities.Factories;
using Entities.Inventory;
using Entities.LocationActors;
using Entities.Model.Inventory;
using Entities.Model.Locations;
using Entities.Model.Markets;
using Entities.NameGenerators;

namespace Entities.Model
{
    /// <summary>
    /// Holds state for the test scenario, this is shared between steps classes within a test scenario execution through constructor injection
    /// </summary>
    public class ScenarioContextState
    {
        /// <summary>
        /// Creates an instance of <see cref="ScenarioContextState"/> class.
        /// </summary>
        public ScenarioContextState()
        {
            Traders = new Dictionary<string, TestActorRef<Trader>>();
            ExchangeContractActors = new Dictionary<string, TestActorRef<ExchangeContract>>();
            TestProbes = new Dictionary<string, TestProbe>();
            Markets = new Dictionary<string, IActorRef>();
            Actors = new Dictionary<string, IActorRef>();
            Materials = new Dictionary<string, IMaterial>();
            CelestialBodies = new Dictionary<string, CelestialBody>();
            FactoryCoordinator = new FactoryCoordinatorScenarioState();
            Config = @"akka {  
                        stdout-loglevel = DEBUG
                        loglevel = DEBUG
                        log-config-on-start = on
                    #     loggers=[""Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog""]
                        actor {
                         debug {
                          receive = on
                          autoreceive = on
                          lifecycle = on
                          event-stream = on
                          unhandled = on
                         }
                        }
                       }";
        }

        public Dictionary<string, IActorRef> Actors { get; set; }

        /// <summary>
        /// A reference to the <see cref="TestKit"/> in order to get at all the Akka.Test goodness
        /// </summary>
        public TestKit TestKit { get; set; }

        /// <summary>
        /// Dictionary of <see cref="Trader"/> by name 
        /// </summary>
        public Dictionary<string, TestActorRef<Trader>> Traders { get; private set; }

        /// <summary>
        /// Dictionary of <see cref="ExchangeContract"/> by name
        /// </summary>
        public Dictionary<string, TestActorRef<ExchangeContract>> ExchangeContractActors { get; private set; }

        public Dictionary<string, TestProbe> TestProbes { get; private set; }
      
        public Dictionary<string, IActorRef> Markets { get; private set; }

        public string Config { get; set; }


        public Dictionary<string, CelestialBody> CelestialBodies { get; set; }
        public Dictionary<string, IMaterial> Materials { get; set; }
        public FactoryCoordinatorScenarioState FactoryCoordinator { get; private set; }

        public InventoryTypeCoordinator Inventory { get; set; }

        public class FactoryCoordinatorScenarioState
        {
            public TimeSpan Period { get; set; }
            public Dictionary<string, FactoryType> FactoryTypes { get; private set; }
            public Dictionary<string, IActorRef> Factories { get; private set; }

            public FactoryCoordinatorScenarioState()
            {
                FactoryTypes = new Dictionary<string, FactoryType>();
                Factories = new Dictionary<string, IActorRef>();
            }
        }

        /// <summary>
        /// Given a resource name will return a <see cref="Resource"/> from <see cref="ResourceManager"/>
        /// </summary>
        /// <param name="resourceName">The value of a <see cref="Resource.Name"/></param>
        /// <exception cref="InvalidOperationException">When <see cref="ResourceManager"/> ha snot been initialised in a previous step.</exception>
        /// <returns>The <see cref="Resource"/></returns>
        public IResource GetResourceFromName(string resourceName)
        {
            var actorRef = Actors[ResourceManager.Name];
            if (ReferenceEquals(actorRef, null))
            {
                throw new InvalidOperationException("Resource Manager not initialised");
            }

            var resourceQuery = actorRef.Ask<ResourceManager.GetResourceResult>(new ResourceManager.GetResource(resourceName));
            resourceQuery.Wait(TimeSpan.FromMilliseconds(50));
            var resource = resourceQuery.Result;
            return resource.Values[0];
        }
    }
}
