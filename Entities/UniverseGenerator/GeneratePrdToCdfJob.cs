using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;

namespace Entities.UniverseGenerator
{
    public class GeneratePdfToCdfJob : ReceiveActor
    {
        public static Props CreateProps()
        {
            return Props.Create(()=>new GeneratePdfToCdfJob());
        }

        public GeneratePdfToCdfJob()
        {
            Receive<Start>(msg =>
            {
                double cumulativeProbability = 0;
                int range = msg.Max - msg.Min;
                // as the range is an int we can see if we can sample every int value or not. If so then number of poitns is meaningless.
                if (range > msg.NoPointsToSample)
                {
                    Tuple<double, int>[] graph = new Tuple<double, int>[msg.NoPointsToSample];
                    double step = range / (double)msg.NoPointsToSample;

                    for (int i = 0; i < range; i++)
                    {
                        int currentVal = (int)Math.Round(msg.Min + i * step);
                        double probability = msg.Function.F(currentVal);
                        cumulativeProbability += probability;
                        graph[i] = new Tuple<double, int>(cumulativeProbability, currentVal);
                    }

                    Cdf cdf = Cdf.GenerateFromHistogram(graph);
                    Sender.Tell(new CdfGenerated(msg.CdfName, cdf));
                }
                else
                {
                    Tuple<double, int>[] graph = new Tuple<double, int>[range];
                    for (int i = 0; i < range; i++)
                    {
                        int currentVal = msg.Min + i;
                        double probability = msg.Function.F(currentVal);
                        cumulativeProbability += probability;
                        graph[i] = new Tuple<double, int>(cumulativeProbability, currentVal);
                    }

                    var cdf = Cdf.GenerateFromHistogram(graph);
                    Sender.Tell(new CdfGenerated(msg.CdfName, cdf));
                }
            });
        }

        public class CdfGenerated
        {
            public string CdfName { get;  }
            public ISingleVariableFunction<int, double> CDF { get;  }

            public CdfGenerated(string cdfName, ISingleVariableFunction<int, double> cdf)
            {
                if (cdf == null) throw new ArgumentNullException(nameof(cdf));
                if (String.IsNullOrWhiteSpace(cdfName)) throw new ArgumentException("Argument is null or whitespace", nameof(cdfName));
              

                CdfName = cdfName;
                CDF = cdf;
            }
        }

        public class Start
        {
            public ISingleVariableFunction<double, int> Function { get; }
            public int Min { get; }
            public int Max { get; }

            /// <summary>
            /// This is the number of points on the x axis between the min and max ranges
            /// </summary>
            public int NoPointsToSample { get; }
            public string CdfName { get; }

            public Start(ISingleVariableFunction<double, int> function, int min, int max, int noPointsToSample, string cdfName)
            {
                if (function == null) throw new ArgumentNullException(nameof(function));
                if (noPointsToSample <= 0) throw new ArgumentOutOfRangeException(nameof(noPointsToSample));
                if (String.IsNullOrWhiteSpace(cdfName)) throw new ArgumentException("Argument is null or whitespace", nameof(cdfName));

                Function = function;
                Min = min;
                Max = max;
                NoPointsToSample = noPointsToSample;
                CdfName = cdfName;
            }
        }
    }
}