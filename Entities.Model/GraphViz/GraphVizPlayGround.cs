using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shields.GraphViz.Components;
using Shields.GraphViz.Models;
using Shields.GraphViz.Services;

namespace Entities.Model.GraphViz
{
    [TestFixture]
    class GraphVizPlayGround
    {
        [TestCase()]
        public void BasicTest()
        {
            Graph graph = Graph.Undirected
                .Add(EdgeStatement.For("a", "b"))
                .Add(EdgeStatement.For("b", "c"))
                .Add(EdgeStatement.For("a", "a"));
            string graphVizBin = @"C:\Program Files (x86)\Graphviz2.38\bin";
        IRenderer renderer = new Renderer(graphVizBin);

            string directory = @"M:\programming\general\git\EconomySimulator\TradingAISimulator\Entities.Model\bin\Debug\GRaphs\";
            string fileName = @"balancedBoundingTreeTests.RotateLeft.png";
            Directory.CreateDirectory(directory);
            using (Stream file = File.Create(directory + fileName))
            {
                 var task = renderer.RunAsync(
                    graph, file,
                    RendererLayouts.Dot,
                    RendererFormats.Png,
                    CancellationToken.None);

                task.Wait();
                Console.WriteLine("file://" + directory + fileName);

                Assert.IsTrue(task.IsCompleted);
                Assert.IsTrue(task.Exception == null);

               
            }
        }
    }
}
