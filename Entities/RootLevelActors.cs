using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Entities.Factories;
using Entities.Inventory;
using Entities.LocationActors;
using Entities.RNG;

namespace Entities
{
    public static class RootLevelActors
    {
        public static ActorSystem ActorSystem { get; private set; }

        public static IActorRef HeartBeatActorRef { get; set; }

        public static IActorRef ResourceManagerActorRef { get; set; }

        public static IActorRef FactoryCoordinatorActorRef { get; set; }

        public static IActorRef InventoryTypeCoordinatorActorRef { get; set; }

        public static IActorRef CenterOfMassManagerActorRef { get; set; }

        public static void SetupRootLevelActors(Random random)
        {
            HeartBeatActorRef = ActorSystem.ActorOf(HeartBeatActor.CreateProps(), HeartBeatActor.Name);
            ResourceManagerActorRef = ActorSystem.ActorOf(ResourceManager.CreateProps(), ResourceManager.Name);
            FactoryCoordinatorActorRef = ActorSystem.ActorOf(FactoryCoordinatorActor.CreateProps(HeartBeatActorRef), FactoryCoordinatorActor.Name);
            InventoryTypeCoordinatorActorRef = ActorSystem.ActorOf(InventoryTypeCoordinator.CreateProps(), InventoryTypeCoordinator.Name);
            CenterOfMassManagerActorRef = ActorSystem.ActorOf(CenterOfMassManagerActor.CreateProps(FactoryCoordinatorActorRef), CenterOfMassManagerActor.Name);

            if (random == null)
            {
                random = new Random();
            }

            RandomActors.SetupRandomActors(random);
        }

        public static void SetActorSystem(ActorSystem actorSystem)
        {
            ActorSystem = actorSystem;
        }

        public static class RandomActors
        {
            public static void SetupRandomActors(Random random)
            {
                if (random == null) throw new ArgumentNullException(nameof(random));
                RandomIntActorRef = ActorSystem.ActorOf(RandomIntActor.CreateProps(random), RandomIntActor.Name);
                RandomDoubleActorRef = ActorSystem.ActorOf(RandomDoubleActor.CreateProps(random), RandomDoubleActor.Name);
            }

            public static IActorRef RandomDoubleActorRef { get; set; }

            public static IActorRef RandomIntActorRef { get; set; }
        }

      
    }
}
