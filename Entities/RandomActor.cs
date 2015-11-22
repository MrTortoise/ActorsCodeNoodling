using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Entities
{
    /// <summary>
    /// Wraps random to avoid the thread safety bullshit.
    /// </summary>
    public class RandomActor : ReceiveActor
    {
        private readonly Random _random;


        public static string Path => @"user/Random";

        public static string Name => "Random";

        public static Props CreateProps(Random random)
        {
            return Props.Create(() => new RandomActor(random));
        }

        public RandomActor(Random random)
        {
            _random = random;

            Receive<NextRandom>(msg =>
            {
                int[] numbers = new int[msg.NumberOfNumbers];
                for (int i = 0; i < msg.NumberOfNumbers; i++)
                {
                    numbers[i] = _random.Next(msg.MinValue, msg.MaxValue);
                }
                
                Sender.Tell(new RandomResult(numbers));
            });
        }

        public class RandomResult
        {
            public int[] Number { get; private set; }

            public RandomResult(int[] number)
            {
                Number = number;
            }
        }

        public class NextRandom
        {
            public int MinValue { get; private set; }
            public int MaxValue { get; private set; }
            public int NumberOfNumbers { get; private set; }

            public NextRandom(int minValue, int maxValue, int numberOfNumbers)
            {
                MinValue = minValue;
                MaxValue = maxValue;
                NumberOfNumbers = numberOfNumbers;
            }
        }
    }
}
