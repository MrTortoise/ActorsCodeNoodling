using System;
using System.Threading.Tasks;
using Akka.Actor;

namespace Entities.RNG
{
    /// <summary>
    /// Wraps random to avoid the thread safety bullshit. 
    /// </summary>
    public class RandomDoubleActor : ReceiveActor
    {
        private readonly Random _random;

        public static string Path => @"user/RandomDouble";

        public static string Name => "RandomDouble";

        public static Props CreateProps(Random random)
        {
            return Props.Create(() => new RandomDoubleActor(random));
        }

        public RandomDoubleActor(Random random)
        {
            _random = random;

            Receive<NextRandom>(msg =>
            {
                var sender = Sender;
                Task.Run(() =>
                {
                    double[] numbers = new double[msg.NumberOfNumbers];
                    for (int i = 0; i < msg.NumberOfNumbers; i++)
                    {
                        numbers[i] = _random.NextDouble();
                    }
                    return new RandomResult(numbers);
                }).PipeTo(sender);
            });
        }

        public class RandomResult
        {
            public double[] Numbers { get; private set; }

            public RandomResult(double[] numbers)
            {
                Numbers = numbers;
            }
        }

        public class NextRandom
        {
            public int NumberOfNumbers { get; private set; }

            public NextRandom(int numberOfNumbers)
            {
                NumberOfNumbers = numberOfNumbers;
            }
        }
    }
}