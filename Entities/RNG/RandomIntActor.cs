using System;
using System.Threading.Tasks;
using Akka.Actor;

namespace Entities.RNG
{
    /// <summary>
    /// Wraps random to avoid the thread safety bullshit. 
    /// </summary>
    public class RandomIntActor : ReceiveActor
    {
        private readonly Random _random;

        public static string Path => @"user/RandomInt";

        public static string Name => "RandomInt";

        public static Props CreateProps(Random random)
        {
            return Props.Create(() => new RandomIntActor(random));
        }

        public RandomIntActor(Random random)
        {
            _random = random;

            Receive<NextRandom>(msg =>
            {
                var sender = Sender;
                Task.Run(() =>
                {
                    int[] numbers = new int[msg.NumberOfNumbers];
                    for (int i = 0; i < msg.NumberOfNumbers; i++)
                    {
                        numbers[i] = _random.Next(msg.MinValue, msg.MaxValue);
                    }
                    return new RandomResult(numbers);
                }).PipeTo(sender);
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
