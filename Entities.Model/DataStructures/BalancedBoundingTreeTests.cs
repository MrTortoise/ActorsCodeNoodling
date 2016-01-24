using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Entities.DataStructures;
using JetBrains.dotMemoryUnit.Util;
using NUnit.Framework;
using Serilog;

namespace Entities.Model.DataStructures
{
    [TestFixture]
    class BalancedBoundingTreeTests
    {
        private const string MProgrammingGeneralGitEconomysimulatorTradingaisimulatorEntitiesModelBinDebugGraphs = @"M:\programming\general\git\EconomySimulator\TradingAISimulator\Entities.Model\bin\Debug\Graphs\";

        [SetUp]
        public void SetUp()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }

        [TestCase()]
        public void AssertDefaultNil()
        {
            var ut = BalancedBoundingTree<double, object>.Nil;
            Assert.AreSame(BalancedBoundingTree<double, object>.Nil, ut.LowerTree);
            Assert.AreSame(BalancedBoundingTree<double, object>.Nil, ut.UpperTree);
        }

        [TestCase()]
        public void CreateTreeBasicAsserts()
        {
            var ut = new BalancedBoundingTree<double, object>(10, 20, new object());
            Assert.IsTrue(ut.Root.Parent == BalancedBoundingTree<double, object>.Nil);
        }

        [TestCase()]
        public void AddSmallNodeToTree()
        {
            var ut =new BalancedBoundingTree<double, object>(100, 200, new object());
            var node = ut.Insert(50, 100, new object());

            Assert.AreEqual(50, ut.Root.LowerTree.LowerBound);
            Assert.AreEqual(100, ut.Root.LowerTree.UpperBound);

            Assert.AreSame(ut.Root,ut.Root.LowerTree.Parent);
        }

        [TestCase()]
        public void AddLargeNodeToTree()
        {
            var ut = new BalancedBoundingTree<double, object>(100, 200, new object());

            ut.Insert(200, 300, new object());

            Assert.AreEqual(200, ut.Root.UpperTree.LowerBound);
            Assert.AreEqual(300, ut.Root.UpperTree.UpperBound);

            Assert.AreSame(ut.Root, ut.Root.UpperTree.Parent);
        }

        [TestCase()]
        public void MinimumTest()
        {
            var tree = new BalancedBoundingTree<double,object>(100, 200, new object());

            tree.Insert(50, 100, new object());
            tree.Insert(200, 250, new object());
            tree.Insert(25, 50, new object());
            tree.Insert(15, 25, new object());

            Assert.AreEqual(15, tree.Minimum().LowerBound);
        }

        [TestCase()]
        public void MaximumCase()
        {
            var tree = new BalancedBoundingTree<double, object>(100, 200, new object());

            tree.Insert(50, 100, new object());
            tree.Insert(25, 50, new object());
            tree.Insert(250, 300, new object());
            tree.Insert(200, 250, new object());

            Assert.AreEqual(300, tree.Maximum().UpperBound);
        }

        /// <summary>
        /// Testing rotate left implementation
        /// </summary>
        /// <remarks>
        /// This is based on p313 of intro to algorithms in red balck trees.
        /// </remarks>
        [TestCase()]
        public void RotateLeft()
        {
            var value = new object();
            var ut = new BalancedBoundingTree<double, object>(100, 200, value); //x
            var x = ut.Root;

            var y = new BalancedBoundingTree<double, object>.BalancedBoundingNode(300, 400, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            ut.Root.UpperTree = y;
            y.Parent = ut.Root;

            var lambda = new BalancedBoundingTree<double, object>.BalancedBoundingNode(400, 500, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            y.UpperTree = lambda;
            lambda.Parent = y;

            var beta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(200, 300, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            y.LowerTree = beta;
            beta.Parent = y;

            var alpha = new BalancedBoundingTree<double, object>.BalancedBoundingNode(50, 100, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            x.LowerTree = alpha;
            alpha.Parent = x;

            // log before
            const string directory = @"M:\programming\general\git\EconomySimulator\TradingAISimulator\Entities.Model\bin\Debug\Graphs\";
            string beforeFileName = @"balancedBoundingTreeTests.RotateLeft.png";
            ut.Write(10, directory, beforeFileName, Log.Logger).Wait();

            ut.LeftRotate(ut.Root);
           
            //log after
            string afterFileName = @"balancedBoundingTreeTests.RotateLeft.Result.png";
            ut.Write(10, directory, afterFileName, Log.Logger).Wait();

            Assert.AreSame(y, ut.Root);
            Assert.AreSame(BalancedBoundingTree<double, object>.Nil, y.Parent);

            Assert.AreSame(lambda,y.UpperTree);
            Assert.AreSame(y, lambda.Parent);

            Assert.AreSame(x, y.LowerTree);
            Assert.AreSame(y,x.Parent);

            Assert.AreSame(beta,x.UpperTree);
            Assert.AreSame(x, beta.Parent);

            Assert.AreSame(alpha, x.LowerTree);
            Assert.AreSame(x, alpha.Parent);
        }

        [TestCase()]
        public void RotateRight()
        {
            var value = new object();
            var ut = new BalancedBoundingTree<double, object>(300, 400, value); //y
            var y = ut.Root;

            var x = new BalancedBoundingTree<double, object>.BalancedBoundingNode(100, 200, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            y.LowerTree = x;
            x.Parent = y;

            var lambda = new BalancedBoundingTree<double, object>.BalancedBoundingNode(400, 500, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            y.UpperTree = lambda;
            lambda.Parent = y;

            var beta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(200, 300, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            x.UpperTree = beta;
            beta.Parent = x;

            var alpha = new BalancedBoundingTree<double, object>.BalancedBoundingNode(50, 100, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            x.LowerTree = alpha;
            alpha.Parent = x;

            // log before
            string beforeFileName = @"balancedBoundingTreeTests.RotateRight.png";
            ut.Write(10, MProgrammingGeneralGitEconomysimulatorTradingaisimulatorEntitiesModelBinDebugGraphs, beforeFileName, Log.Logger).Wait();

            ut.RightRotate(ut.Root);

            //log after
            string afterFileName = @"balancedBoundingTreeTests.RotateRight.Result.png";
            ut.Write(10, MProgrammingGeneralGitEconomysimulatorTradingaisimulatorEntitiesModelBinDebugGraphs, afterFileName, Log.Logger).Wait();

            Assert.AreSame(x, ut.Root);
            Assert.AreSame(BalancedBoundingTree<double, object>.Nil, x.Parent);

            Assert.AreSame(x.LowerTree, alpha);
            Assert.AreSame(alpha.Parent, x);

            Assert.AreSame(y, x.UpperTree);
            Assert.AreSame(x, y.Parent);

            Assert.AreSame(beta, y.LowerTree);
            Assert.AreSame(y, beta.Parent);

            Assert.AreSame(lambda, y.UpperTree);
            Assert.AreSame(y, lambda.Parent);
        }

        /// <summary>
        /// Based on an example on page 317 13.3 Insertion
        /// </summary>
        [TestCase]
        public void RBInsertFixupLeftHandVersion()
        {
            var value = new object();
            var tree = new BalancedBoundingTree<double,object>(10.5,11.5,value);
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;
            var eleven = tree.Root;
            var two = new BalancedBoundingTree<double, object>.BalancedBoundingNode(1.5,2.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            var one = new BalancedBoundingTree<double, object>.BalancedBoundingNode(0.5, 1.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);
            var seven = new BalancedBoundingTree<double, object>.BalancedBoundingNode(6.5, 7.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);
            var five = new BalancedBoundingTree<double, object>.BalancedBoundingNode(4.5, 5.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            var eight = new BalancedBoundingTree<double, object>.BalancedBoundingNode(7.5, 8.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            var fourteen = new BalancedBoundingTree<double, object>.BalancedBoundingNode(13.5, 14.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);
            var fifteen= new BalancedBoundingTree<double, object>.BalancedBoundingNode(14.5, 15.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);

            var four = new BalancedBoundingTree<double, object>.BalancedBoundingNode(3.5, 4.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);


            tree.Root.UpperTree = fourteen;
            fourteen.Parent = tree.Root;
            fourteen.UpperTree = fifteen;
            fifteen.Parent = fourteen;

            tree.Root.LowerTree = two;
            two.Parent = tree.Root;
            two.LowerTree = one;
            one.Parent = two;
            two.UpperTree = seven;
            seven.Parent = two;

            seven.LowerTree = five;
            five.Parent = seven;
            seven.UpperTree = eight;
            eight.Parent = seven;

            // log before
            string beforeFileName = @"balancedBoundingTreeTests.RBInsertFixupLeftHandVersion.png";
            tree.Write(10, MProgrammingGeneralGitEconomysimulatorTradingaisimulatorEntitiesModelBinDebugGraphs, beforeFileName, Log.Logger).Wait();

            tree.Insert(four);

            // log after
            string afterFileName = @"balancedBoundingTreeTests.RBInsertFixupLeftHandVersion.Result.png";
            tree.Write(10, MProgrammingGeneralGitEconomysimulatorTradingaisimulatorEntitiesModelBinDebugGraphs, afterFileName, Log.Logger).Wait();

            Assert.AreSame(seven, tree.Root);

            Assert.AreSame(eleven,seven.UpperTree);
            Assert.AreSame(seven, eleven.Parent);
            Assert.AreEqual(eleven.NodeColour, red);

            Assert.AreSame(eight, eleven.LowerTree);
            Assert.AreSame(eleven, eight.Parent);
            Assert.AreEqual(black, eight.NodeColour);

            Assert.AreSame(fourteen, eleven.UpperTree);
            Assert.AreSame(eleven, fourteen.Parent);
            Assert.AreEqual(black,fourteen.NodeColour);

            Assert.AreSame(fifteen, fourteen.UpperTree);
            Assert.AreSame(fourteen, fifteen.Parent);
            Assert.AreEqual(red, fifteen.NodeColour);

            Assert.AreSame(two, seven.LowerTree);
            Assert.AreSame(seven, two.Parent);
            Assert.AreEqual(red, two.NodeColour);

            Assert.AreSame(one, two.LowerTree);
            Assert.AreSame(two, one.Parent);
            Assert.AreEqual(black, one.NodeColour);

            Assert.AreSame(five, two.UpperTree);
            Assert.AreEqual(two, five.Parent);
            Assert.AreEqual(black,five.NodeColour);

            Assert.AreSame(four, five.LowerTree);
            Assert.AreSame(five, four.Parent);
            Assert.AreEqual(red, four.NodeColour);
        }

        [TestCase]
        public void RBInsertFixupRightHandVersion()
        {
            var value = new object();
            var tree = new BalancedBoundingTree<double, object>(2.5, 3.5, value);
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;
            var three = tree.Root;
            var two = new BalancedBoundingTree<double, object>.BalancedBoundingNode(1.5, 2.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);
            var one = new BalancedBoundingTree<double, object>.BalancedBoundingNode(0.5, 1.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);

            var nine = new BalancedBoundingTree<double, object>.BalancedBoundingNode(8.5, 9.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            var ten = new BalancedBoundingTree<double, object>.BalancedBoundingNode(9.5, 10.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);
            var eleven = new BalancedBoundingTree<double, object>.BalancedBoundingNode(10.5, 11.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);

            var fourteen = new BalancedBoundingTree<double, object>.BalancedBoundingNode(13.5, 14.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);
            var fifteen = new BalancedBoundingTree<double, object>.BalancedBoundingNode(14.5, 15.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);

            var twelve = new BalancedBoundingTree<double, object>.BalancedBoundingNode(11.5, 12.5, value, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red);


            tree.Root.LowerTree = two;
            two.Parent = tree.Root;

            two.LowerTree = one;
            one.Parent = two;

            tree.Root.UpperTree = fourteen;
            fourteen.Parent = tree.Root;

            fourteen.UpperTree = fifteen;
            fifteen.Parent = fourteen;

            fourteen.LowerTree = ten;
            ten.Parent = fourteen;

            ten.LowerTree = nine;
            nine.Parent = ten;

            ten.UpperTree = eleven;
            eleven.Parent = ten;

            // log before
            string beforeFileName = @"balancedBoundingTreeTests.RBInsertFixupLeftHandVersion.png";
            tree.Write(10, MProgrammingGeneralGitEconomysimulatorTradingaisimulatorEntitiesModelBinDebugGraphs, beforeFileName, Log.Logger).Wait();

            tree.Insert(twelve);

            // log after
            string afterFileName = @"balancedBoundingTreeTests.RBInsertFixupRightHandVersion.Result.png";
            tree.Write(10, MProgrammingGeneralGitEconomysimulatorTradingaisimulatorEntitiesModelBinDebugGraphs, afterFileName, Log.Logger).Wait();

            Assert.AreSame(ten, tree.Root);
            Assert.AreSame(three, ten.LowerTree);
            Assert.AreSame(ten, three.Parent);

            Assert.AreSame(two, three.LowerTree);
            Assert.AreSame(three, two.Parent);

            Assert.AreSame(one, two.LowerTree);
            Assert.AreSame(two, one.Parent);

            Assert.AreSame(nine, three.UpperTree);
            Assert.AreSame(three, nine.Parent);

            Assert.AreSame(fourteen, ten.UpperTree);
            Assert.AreSame(ten, fourteen.Parent);

            Assert.AreSame(eleven, fourteen.LowerTree);
            Assert.AreSame(fourteen, eleven.Parent);

            Assert.AreSame(fifteen, fourteen.UpperTree);
            Assert.AreSame(fourteen, fifteen.Parent);

            Assert.AreSame(twelve, eleven.UpperTree);
            Assert.AreSame(eleven, twelve.Parent);

            Assert.AreEqual(black, ten.NodeColour);
            Assert.AreEqual(red, three.NodeColour);
            Assert.AreEqual(red, fourteen.NodeColour);
            Assert.AreEqual(black, two.NodeColour);
            Assert.AreEqual(black, nine.NodeColour);
            Assert.AreEqual(black, eleven.NodeColour);
            Assert.AreEqual(black, fifteen.NodeColour);
            Assert.AreEqual(red, one.NodeColour);
            Assert.AreEqual(red, twelve.NodeColour);
        }

        [TestCase(50,50,"hi",false)]
        [TestCase(50, 100, "hi", false)]
        [TestCase(100, 50, "hi", true)]
        public void BalancedBoundingNodeConstructorTest(double lower, double upper, string value, bool throws)
        {
            if (throws)
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => new BalancedBoundingTree<double, string>.BalancedBoundingNode(lower, upper, value, BalancedBoundingTree<double, string>.BalancedBoundingNode.Colour.Black));
            }
            else
            {
                var node = new BalancedBoundingTree<double, string>.BalancedBoundingNode(lower, upper, value, BalancedBoundingTree<double, string>.BalancedBoundingNode.Colour.Black);
                Assert.AreEqual(lower,node.LowerBound);
                Assert.AreEqual(upper, node.UpperBound);
                Assert.AreEqual(value, node.Value);
            }
        }

        [TestCase(100, 200, 200, 300, -1)]
        [TestCase(100, 200, 100, 200, 0)]
        [TestCase(100, 200, 50, 100, 1)]
        [TestCase(100, 200, 300, 400, -1)]
        [TestCase(100, 200, 50, 75, 1)]
        public void BalancedBoundingNodeCompareTest(double lhsLower, double lhsUpper, double rhsLower, double rhsUpper, int expected)
        {
            object obj = new object();
            var lhs = new BalancedBoundingTree<double, object>.BalancedBoundingNode(lhsLower, lhsUpper, obj, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);
            var rhs = new BalancedBoundingTree<double, object>.BalancedBoundingNode(rhsLower, rhsUpper, obj, BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black);
            int result = lhs.CompareTo(rhs);
            Assert.AreEqual(expected, result);
        }
    }
}
