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
                Tuple<double, int>[] graph;
                if (range > msg.NoPointsToSample)
                {
                    double step = range/(double) msg.NoPointsToSample;
                    graph = new Tuple<double, int>[msg.NoPointsToSample];
                    for (int i = 0; i < range; i++)
                    {
                        int currentVal = (int) Math.Round(msg.Min + i*step);
                        double probability = msg.Function.F(currentVal);
                        cumulativeProbability += probability;
                        graph[i] = new Tuple<double, int>(cumulativeProbability, currentVal);
                    }
                }
                else
                {
                    graph = new Tuple<double, int>[range];
                    for (int i = 0; i < range; i++)
                    {
                        int currentVal = msg.Min + i;
                        double probability = msg.Function.F(currentVal);
                        cumulativeProbability += probability;
                        graph[i] = new Tuple<double, int>(cumulativeProbability, currentVal);
                    }
                }

                Tuple<double, int>[] normalisedGraph = new Tuple<double, int>[graph.Length];
                double probabilityNormaliser = 1/cumulativeProbability;
                for (var i = graph.Length - 1; i >= 0; i--)
                {
                    normalisedGraph[i] = new Tuple<double, int>(graph[i].Item1*probabilityNormaliser, graph[i].Item2);
                }

                Cdf cdf = Cdf.GenerateFromHistogram(normalisedGraph);
                Sender.Tell(new CdfGenerated(msg.CdfName, cdf));
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