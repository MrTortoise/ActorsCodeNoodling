using System;
using Entities.DataStructures;

namespace Entities.UniverseGenerator
{
    public class Cdf : ISingleVariableFunction<int,double>
    {
        private readonly BalancedBoundingTree<double, int> _graph;

        public static Cdf GenerateFromHistogram(Tuple<double, int>[] graph)
        {
            if (graph.Length == 0) throw new ArgumentException("Argument is empty collection", nameof(graph));

            for (int i = 0; i < graph.Length - 1; i++)
            {
                double lower;
                double upper;
                if (i == 0)
                {
                    lower = Double.MinValue;
                }
                else
                {
                    lower = graph[i - 1].Item1;
                }
            }
            throw new NotImplementedException();
        }

        private Cdf(BalancedBoundingTree<double, int> graph)
        {
            _graph = graph;
        }

        public int F(double x)
        {
            //if (_graph.ContainsKey(x))
            //{
            //    return _graph[x];
            //}

            throw new NotImplementedException();

            //double previous = 0;
            //for (var i = 0; i < _graph.Keys.Count; i++)
            //{
              
            //}
        }


    }
}