using System;
using System.Linq;
using Entities.DataStructures;

namespace Entities.UniverseGenerator
{
    public class Cdf : ISingleVariableFunction<int,double>
    {
        private readonly BalancedBoundingTree<double, int> _graph;

        public static Cdf GenerateFromHistogram(Tuple<double, int>[] graph)
        {
            if (graph.Length == 0) throw new ArgumentException("Argument is empty collection", nameof(graph));

            var min = graph.Select(i => i.Item1).Min();
            if (min != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(graph),$"Graph must contain a 0 value as it is of range 0 and 1:{min}");
            }

            var max = graph.Select(i => i.Item1).Max();
            if (max != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(graph), $"Graph must contain a 1 value as it is of range 0 and 1: {max}");
            }

            BalancedBoundingTree<double, int> tree = new BalancedBoundingTree<double, int>(Double.MinValue, graph[0].Item1, graph[0].Item2);

            for (int i = 0; i < graph.Length - 1; i++)
            {
                tree.Insert(graph[i].Item1, graph[i + 1].Item1, graph[i].Item2);
            }

            var maxTuple = graph[graph.Length-1];
            tree.Insert(maxTuple.Item1, double.MaxValue, maxTuple.Item2);

            return new Cdf(tree);
        }

        private Cdf(BalancedBoundingTree<double, int> graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            _graph = graph;
        }

        public int F(double x)
        {
            if (x < 0 || x > 1) throw new ArgumentOutOfRangeException(nameof(x), $"argument must be 0<=x<=1 : {x}");
            var node = _graph.Search(x);
            return node.Value;
        }
    }
}