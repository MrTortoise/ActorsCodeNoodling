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
            Assert.IsNull(ut.LowerTree);
            Assert.IsNull(ut.UpperTree);
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

            var ut = new BalancedBoundingTree<double, object>(100, 200, new object()); //x
            var x = ut.Root;
            var y = ut.Insert(300, 400, new object()); //y
            var lambda = ut.Insert(400, 500, new object()); //lambda
            var beta = ut.Insert(200, 300, new object()); //beta
            var alpha = ut.Insert(50, 100, new object()); //alpha

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
            var ut = new BalancedBoundingTree<double, object>(300, 400, new object()); //y
            var y = ut.Root;
            var x = ut.Insert(100, 200, new object()); //y
            var lambda = ut.Insert(400, 500, new object()); //lambda
            var beta = ut.Insert(200, 300, new object()); //beta
            var alpha = ut.Insert(50, 100, new object()); //alpha

            // log before
            const string directory = @"M:\programming\general\git\EconomySimulator\TradingAISimulator\Entities.Model\bin\Debug\Graphs\";
            string beforeFileName = @"balancedBoundingTreeTests.RotateRight.png";
            ut.Write(10, directory, beforeFileName, Log.Logger).Wait();

            ut.RightRotate(ut.Root);

            //log after
            string afterFileName = @"balancedBoundingTreeTests.RotateRight.Result.png";
            ut.Write(10, directory, afterFileName, Log.Logger).Wait();

            Assert.AreSame(alpha, ut.Root);
            Assert.AreSame(BalancedBoundingTree<double, object>.Nil, alpha.Parent);

            Assert.AreSame(x, alpha.UpperTree);
            Assert.AreSame(alpha, x.Parent);

            Assert.AreSame(y, x.UpperTree);
            Assert.AreSame(x, y.Parent);

            Assert.AreSame(beta, y.LowerTree);
            Assert.AreSame(y, beta.Parent);

            Assert.AreSame(lambda, y.UpperTree);
            Assert.AreSame(y, lambda.Parent);
        }

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
            fourteen.UpperTree = fifteen;

            tree.Root.LowerTree = two;
            two.LowerTree = one;
            two.UpperTree = seven;

            seven.LowerTree = five;
            seven.UpperTree = eight;

            tree.Insert(3.5, 4.5, value);

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
