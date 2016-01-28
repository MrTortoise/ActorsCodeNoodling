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
        private const string TestGraphDirectory = @"M:\programming\general\git\EconomySimulator\TradingAISimulator\Entities.Model\bin\Debug\Graphs\";
        
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
            var ut = new BalancedBoundingTree<double, object>(100, 200, new object());
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
            ut.Write(10, TestGraphDirectory, beforeFileName, Log.Logger).Wait();

            ut.RightRotate(ut.Root);

            //log after
            string afterFileName = @"balancedBoundingTreeTests.RotateRight.Result.png";
            ut.Write(10, TestGraphDirectory, afterFileName, Log.Logger).Wait();

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
        public void RbInsertFixupLeftHandVersion()
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
            string beforeFileName = @"balancedBoundingTreeTests.RbInsertFixupLeftHandVersion.png";
            tree.Write(10, TestGraphDirectory, beforeFileName, Log.Logger).Wait();

            tree.Insert(four);

            // log after
            string afterFileName = @"balancedBoundingTreeTests.RbInsertFixupLeftHandVersion.Result.png";
            tree.Write(10, TestGraphDirectory, afterFileName, Log.Logger).Wait();

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
        public void RbInsertFixupRightHandVersion()
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
            string beforeFileName = @"balancedBoundingTreeTests.RbInsertFixupLeftHandVersion.png";
            tree.Write(10, TestGraphDirectory, beforeFileName, Log.Logger).Wait();

            tree.Insert(twelve);

            // log after
            string afterFileName = @"balancedBoundingTreeTests.RbInsertFixupRightHandVersion.Result.png";
            tree.Write(10, TestGraphDirectory, afterFileName, Log.Logger).Wait();

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

        [Test]
        public void InsertFixupCase1ATest()
        {
            var value = new object();
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var tree = new BalancedBoundingTree<double, object>(9.5, 10.5, value);
            var c = tree.Root;
            var root = new BalancedBoundingTree<double, object>.BalancedBoundingNode(1, 2, value, black);
            tree.Root = root;
            c.Parent = root;
            root.UpperTree = c;


            var a = new BalancedBoundingTree<double, object>.BalancedBoundingNode(4.5, 5.5, value, red);
            a.Parent = c;
            c.LowerTree = a;

            var b = new BalancedBoundingTree<double, object>.BalancedBoundingNode(7.5, 8.5, value, red);
            b.Parent = a;
            a.UpperTree = b;

            var d = new BalancedBoundingTree<double, object>.BalancedBoundingNode(14.5, 15.5, value, red);
            d.Parent = c;
            c.UpperTree = d;

          
            var alpha = new BalancedBoundingTree<double, object>.BalancedBoundingNode(3.5, 4.5, value, black);
            alpha.Parent = a;
            a.LowerTree = alpha;

            var beta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(6.5, 7.5, value, black);
            beta.Parent = b;
            b.LowerTree = beta;

            var lambda = new BalancedBoundingTree<double, object>.BalancedBoundingNode(8.5, 9.5, value, black);
            lambda.Parent = b;
            b.UpperTree = lambda;

            var theta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(13.5, 14.5, value, black);
            theta.Parent = d;
            d.LowerTree = theta;

            var epsilon = new BalancedBoundingTree<double, object>.BalancedBoundingNode(15.5, 16.5, value, black);
            epsilon.Parent = d;
            d.UpperTree = epsilon;

            tree.FixupNode(b);

            Assert.AreEqual(red, tree.Root.UpperTree.NodeColour);
            Assert.AreEqual(black,a.NodeColour);
            Assert.AreEqual(red,b.NodeColour);
            Assert.AreEqual(black, d.NodeColour);
        }

        [Test]
        public void InsertFixupCase1BTest()
        {
            var value = new object();
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var tree = new BalancedBoundingTree<double, object>(9.5, 10.5, value);
            var c = tree.Root;

            var root =  new BalancedBoundingTree<double, object>.BalancedBoundingNode(1, 2 ,value, black);
            tree.Root = root;
            root.UpperTree = c;
            c.Parent = root;

    

            var b = new BalancedBoundingTree<double, object>.BalancedBoundingNode(7.5, 8.5, value, red);
            b.Parent = c;
            c.LowerTree= b;

            var a = new BalancedBoundingTree<double, object>.BalancedBoundingNode(4.5, 5.5, value, red);
            a.Parent = b;
            b.LowerTree = a;

            var d = new BalancedBoundingTree<double, object>.BalancedBoundingNode(14.5, 15.5, value, red);
            d.Parent = c;
            c.UpperTree = d;


            var alpha = new BalancedBoundingTree<double, object>.BalancedBoundingNode(3.5, 4.5, value, black);
            alpha.Parent = a;
            a.LowerTree = alpha;

            var beta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(6.5, 7.5, value, black);
            beta.Parent = a;
            a.UpperTree= beta;

            var lambda = new BalancedBoundingTree<double, object>.BalancedBoundingNode(8.5, 9.5, value, black);
            lambda.Parent = b;
            b.UpperTree = lambda;

            var theta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(13.5, 14.5, value, black);
            theta.Parent = d;
            d.LowerTree = theta;

            var epsilon = new BalancedBoundingTree<double, object>.BalancedBoundingNode(15.5, 16.5, value, black);
            epsilon.Parent = d;
            d.UpperTree = epsilon;

            tree.FixupNode(a);

            Assert.AreEqual(red, tree.Root.UpperTree.NodeColour);
            Assert.AreEqual(red, a.NodeColour);
            Assert.AreEqual(black, b.NodeColour);
            Assert.AreEqual(black, d.NodeColour);
        }

        [Test]
        public void InsertFixupCase2And3Test()
        {
            var value = new object();
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var tree = new BalancedBoundingTree<double, object>(12.5, 13.5, value);
            var c = tree.Root;
            var root = new BalancedBoundingTree<double, object>.BalancedBoundingNode(1, 2, value, black);
            tree.Root = root;
            c.Parent = root;
            root.UpperTree = c;


            var a = new BalancedBoundingTree<double, object>.BalancedBoundingNode(4.5, 5.5, value, red);
            a.Parent = c;
            c.LowerTree = a;

            var b = new BalancedBoundingTree<double, object>.BalancedBoundingNode(8.5, 9.5, value, red);
            b.Parent = a;
            a.UpperTree= b;

            var alpha = new BalancedBoundingTree<double, object>.BalancedBoundingNode(3.5, 4.5, value, black);
            alpha.Parent = a;
            a.LowerTree = alpha;

            var beta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(6.5, 7.5, value, black);
            beta.Parent = b;
            b.LowerTree = beta;

            var lambda = new BalancedBoundingTree<double, object>.BalancedBoundingNode(9.5, 10.5, value, black);
            lambda.Parent = b;
            b.UpperTree = lambda;

            var theta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(13.5, 14.5, value, black);
            theta.Parent = c;
            c.UpperTree = theta;

            // log before
            string beforeFileName = @"balancedBoundingTreeTests.InsertFixupCase2And3Test.png";
            tree.Write(10, TestGraphDirectory, beforeFileName, Log.Logger).Wait();

            tree.FixupNode(b);

            // log after
            string afterFileName = @"balancedBoundingTreeTests.InsertFixupCase2And3Test.Result.png";
            tree.Write(10, TestGraphDirectory, afterFileName, Log.Logger).Wait();

            Assert.AreSame(b, tree.Root.UpperTree);
            Assert.AreSame(a, b.LowerTree);
            Assert.AreSame(c, b.UpperTree);
            Assert.AreSame(alpha, a.LowerTree);
            Assert.AreSame(beta, a.UpperTree);
            Assert.AreSame(lambda, c.LowerTree);
            Assert.AreSame(theta, c.UpperTree);

            Assert.AreEqual(black, tree.Root.UpperTree.NodeColour);
            Assert.AreEqual(red, a.NodeColour);
            Assert.AreEqual(red, c.NodeColour);
        }

        [Test]
        public void InsertFixupCase2And3ReflectedTest()
        {
            var value = new object();
            var tree = new BalancedBoundingTree<double, object>(9.5, 10.5, value);
            var c = tree.Root;

            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var a = new BalancedBoundingTree<double, object>.BalancedBoundingNode(14.5, 15.5, value, red);
            a.Parent = c;
            c.UpperTree = a;

            var b = new BalancedBoundingTree<double, object>.BalancedBoundingNode(12.5, 13.5, value, red);
            b.Parent = a;
            a.LowerTree = b;

            var alpha = new BalancedBoundingTree<double, object>.BalancedBoundingNode(16.5, 17.5, value, black);
            alpha.Parent = a;
            a.UpperTree = alpha;

            var beta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(13.5, 14.5, value, black);
            beta.Parent = b;
            b.UpperTree = beta;

            var lambda = new BalancedBoundingTree<double, object>.BalancedBoundingNode(11.5, 12.5, value, black);
            lambda.Parent = b;
            b.LowerTree = lambda;

            var theta = new BalancedBoundingTree<double, object>.BalancedBoundingNode(8.5, 9.5, value, black);
            theta.Parent = c;
            c.LowerTree = theta;

            // log before
            string beforeFileName = @"balancedBoundingTreeTests.InsertFixupCase2And3ReflectTest.png";
            tree.Write(10, TestGraphDirectory, beforeFileName, Log.Logger).Wait();

            tree.FixupNode(b);

            // log after
            string afterFileName = @"balancedBoundingTreeTests.InsertFixupCase2And3ReflectTest.Result.png";
            tree.Write(10, TestGraphDirectory, afterFileName, Log.Logger).Wait();

            Assert.AreSame(b, tree.Root);
            Assert.AreSame(a, b.UpperTree);
            Assert.AreSame(c, b.LowerTree);
            Assert.AreSame(alpha, a.UpperTree);
            Assert.AreSame(beta, a.LowerTree);
            Assert.AreSame(lambda, c.UpperTree);
            Assert.AreSame(theta, c.LowerTree);

            Assert.AreEqual(black, tree.Root.NodeColour);
            Assert.AreEqual(red, a.NodeColour);
            Assert.AreEqual(red, c.NodeColour);
        }

        [Test]
        public void TreeInsertsAscending1()
        {
            var value = new object();
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var tree = new BalancedBoundingTree<double, object>(0, 0.1, value);
            Assert.AreEqual(black, tree.Root.NodeColour);
            Assert.AreEqual(0,tree.Root.LowerBound);
            Assert.AreEqual(0.1, tree.Root.UpperBound);
        }

        [Test]
        public void TreeInsertsAscending2()
        {
            var value = new object();
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var tree = new BalancedBoundingTree<double, object>(0, 0.1, value);
            tree.Insert(0.1, 0.2, value);

            Assert.AreEqual(black, tree.Root.NodeColour);
            Assert.AreEqual(0, tree.Root.LowerBound);
            Assert.AreEqual(0.1, tree.Root.UpperBound);

            var two = tree.Root.UpperTree;
            Assert.AreEqual(red, two.NodeColour);
            Assert.AreEqual(0.1,two.LowerBound);
            Assert.AreEqual(0.2, two.UpperBound);
        }

        [Test]
        public void TreeInsertsAscending3()
        {
            var value = new object();
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var tree = new BalancedBoundingTree<double, object>(0, 0.1, value);
            tree.Insert(0.1, 0.2, value);
            tree.Insert(0.2, 0.3, value);

            Assert.AreEqual(black, tree.Root.NodeColour);
            Assert.AreEqual(0.1, tree.Root.LowerBound);
            Assert.AreEqual(0.2, tree.Root.UpperBound);

            var one = tree.Root.LowerTree;
            Assert.AreEqual(red, one.NodeColour);
            Assert.AreEqual(0, one.LowerBound);
            Assert.AreEqual(0.1, one.UpperBound);

            var three = tree.Root.UpperTree;
            Assert.AreEqual(red, three.NodeColour);
            Assert.AreEqual(0.2,three.LowerBound);
            Assert.AreEqual(0.3,three.UpperBound);
        }

        [Test]
        public void TreeInsertsAscending4()
        {
            var value = new object();
            var red = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Red;
            var black = BalancedBoundingTree<double, object>.BalancedBoundingNode.Colour.Black;

            var tree = new BalancedBoundingTree<double, object>(0, 0.1, value);
            tree.Insert(0.1, 0.2, value);
            tree.Insert(0.2, 0.3, value);
            tree.Insert(0.3, 0.4, value);

            Assert.AreEqual(black, tree.Root.NodeColour);
            Assert.AreEqual(0.1, tree.Root.LowerBound);
            Assert.AreEqual(0.2, tree.Root.UpperBound);

            var one = tree.Root.LowerTree;
            Assert.AreEqual(black, one.NodeColour);
            Assert.AreEqual(0, one.LowerBound);
            Assert.AreEqual(0.1, one.UpperBound);

            var three = tree.Root.UpperTree;
            Assert.AreEqual(black, three.NodeColour);
            Assert.AreEqual(0.2, three.LowerBound);
            Assert.AreEqual(0.3, three.UpperBound);

            var four = three.UpperTree;
            Assert.AreEqual(red, four.NodeColour);
            Assert.AreEqual(0.3, four.LowerBound);
            Assert.AreEqual(0.4, four.UpperBound);
        }

        [TestCase(0.05, 1)]
        [TestCase(0.15, 2)]
        [TestCase(0.25, 3)]
        [TestCase(0.35, 4)]
        [TestCase(0.45,null)]
        [TestCase(0,1)]
        [TestCase(0.4,4)]
        [TestCase(0.2,3)]
        public void SearchTests(double searchValue, int? foundValue)
        {
            var tree = new BalancedBoundingTree<double, int>(0, 0.1, 1);
            tree.Insert(0.1, 0.2, 2);
            tree.Insert(0.2, 0.3, 3);
            tree.Insert(0.3, 0.4, 4);

            if (!foundValue.HasValue)
            {
                Assert.AreSame(BalancedBoundingTree<double, int>.Nil, tree.Search(searchValue));
            }
            else
            {
                var actual = tree.Search(searchValue);
                Assert.AreEqual(foundValue.Value, actual.Value);
            }
        }
    }
}
