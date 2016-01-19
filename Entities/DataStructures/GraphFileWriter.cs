using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Shields.GraphViz.Components;
using Shields.GraphViz.Models;
using Shields.GraphViz.Services;

namespace Entities.DataStructures
{
    /// <summary>
    /// responsible for walking and then redering a png image of the tree. Attach a logger in debug mode for file path.
    /// </summary>
    public static class GraphFileWriter
    {
        public static async Task Write<TBounds, TPayload>(this BalancedBoundingTree<TBounds, TPayload> tree, int depth, string directory, string fileName, ILogger logger = null)
            where TBounds : IComparable<TBounds>, IEquatable<TBounds>
        {
            var graph = tree.ConstructGraph(depth);

            DirectoryInfo dir = new DirectoryInfo(directory);
            dir.Create();
            string graphVizBin = @"C:\Program Files (x86)\Graphviz2.38\bin";
            IRenderer renderer = new Renderer(graphVizBin);
            using (Stream file = File.Create(directory + fileName))
            {
                await renderer.RunAsync(
                    graph, file,
                    RendererLayouts.Dot,
                    RendererFormats.Png,
                    CancellationToken.None);
            }
            logger?.Debug("Graph written to: file://" + directory + fileName);
        }

        private static Graph ConstructGraph<TBounds, TPayload>(this BalancedBoundingTree<TBounds, TPayload> tree, int depth)
            where TBounds : IComparable<TBounds>, IEquatable<TBounds>
        {
            Graph graph = Graph.Undirected;
            if (depth > 0)
            {
                graph = tree.Root.AddToGraph(graph, depth);
            }
            return graph;
        }

        private static Graph AddToGraph<TBounds, TPayload>(this BalancedBoundingTree<TBounds, TPayload>.BalancedBoundingNode node, Graph graph, int depth)
            where TBounds : IComparable<TBounds>, IEquatable<TBounds>
        {
            if (!ReferenceEquals(node.LowerTree, BalancedBoundingTree < TBounds, TPayload > .Nil))
            {
                graph = graph.Add(EdgeStatement.For(node.ToString(), node.LowerTree.ToString()));
                if (depth > 0)
                {
                    graph = node.LowerTree.AddToGraph(graph, depth - 1);
                }
            }

            if (!ReferenceEquals(node.UpperTree, BalancedBoundingTree < TBounds, TPayload >. Nil))
            {
                graph = graph.Add(EdgeStatement.For(node.ToString(), node.UpperTree.ToString()));
                if (depth > 0)
                {
                    graph = node.UpperTree.AddToGraph(graph, depth - 1);
                }
            }

            return graph;
        }
    }
}