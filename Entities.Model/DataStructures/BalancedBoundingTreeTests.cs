using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Entities.Model.DataStructures
{
    [TestFixture]
    class BalancedBoundingTreeTests
    {
        [TestCase()]
        public void AssertDefaultNil()
        {
            var ut = BalancedBoundingNode<object>.Create(100, 200, new object());
            Assert.IsNull(ut.LowerTree);
            Assert.IsNull(ut.UpperTree);
        }

        [TestCase()]
        public void AddSmallNodeToTree()
        {
            var ut = BalancedBoundingNode<object>.Create(100, 200, new object());
            ut.Insert(50, 100, new object());

            Assert.AreEqual(50, ut.LowerTree.LowerBound);
            Assert.AreEqual(100, ut.LowerTree.UpperBound);
        }

        [TestCase()]
        public void AddLargeNodeToTree()
        {
            var ut = BalancedBoundingNode<object>.Create(100, 200, new object());
            Assert.IsNull(ut.LowerTree);
            Assert.IsNull(ut.UpperTree);
            ut.Insert(50, 100, new object());

            Assert.AreEqual(50, ut.LowerTree.LowerBound);
            Assert.AreEqual(100, ut.LowerTree.UpperBound);
        }

        [TestCase()]
        public void MinimumTest()
        {
            var ut = BalancedBoundingNode<object>.Create(100, 200, new object());

            ut.Insert(50, 100, new object());
            ut.Insert(200, 250, new object());
            ut.Insert(25, 50, new object());
            ut.Insert(15, 25, new object());

            Assert.AreEqual(15, ut.Minimum().LowerBound);
        }

        [TestCase()]
        public void MaximumCase()
        {
            var ut = BalancedBoundingNode<object>.Create(100, 200, new object());

            ut.Insert(50, 100, new object());
            ut.Insert(25, 50, new object());
            ut.Insert(250, 300, new object());
            ut.Insert(200, 250, new object());

            Assert.AreEqual(300, ut.Maximum().UpperBound);
        }

        [TestCase()]
        public void RotateLeft()
        {

            var ut = BalancedBoundingNode<object>.Create(100, 200, new object()); //x
            ut.Insert(300, 400, new object()); //y
            ut.Insert(400, 500, new object()); //lambda
            ut.Insert(200, 300, new object()); //beta
            ut.Insert(50, 100, new object()); //alpha

            ut.LeftRotate();

            Assert.AreEqual(300, ut.Parent.LowerBound); //y
            Assert.AreEqual(400, ut.Parent.UpperBound); //y

            Assert.AreEqual(100, ut.LowerBound); //x
            Assert.AreEqual(200, ut.UpperBound); //x

            Assert.AreEqual(400, ut.Parent.UpperTree.LowerBound); //lambda
            Assert.AreEqual(500, ut.Parent.UpperTree.UpperBound); //lambda

            Assert.AreEqual(200, ut.UpperTree.LowerBound); //beta
            Assert.AreEqual(300, ut.UpperTree.UpperBound); // beta

            Assert.AreEqual(50, ut.LowerTree.LowerBound); //alpha
            Assert.AreEqual(100, ut.LowerTree.UpperBound); //alpha
        }
    }


    public class BalancedBoundingNode<T>
    {
        public BalancedBoundingNode<T> Parent { get; private set; } 
        public double LowerBound { get;  }
        public double UpperBound { get;  }
        public T Value { get;  }
        public Colour NodeColour { get; }

        public enum Colour
        {
            Black,
            Red
        }

        public BalancedBoundingNode<T> LowerTree { get; private set; } 
        public BalancedBoundingNode<T> UpperTree { get; private set; } 

        public static BalancedBoundingNode<T> Create(double lowerBound, double upperBound, T value)
        {
            return new BalancedBoundingNode<T>(lowerBound, upperBound, value, Colour.Black, null);
        }

        internal BalancedBoundingNode(double lower, double upper, T value, Colour nodeColour, BalancedBoundingNode<T> parent)
        {
            LowerBound = lower;
            UpperBound = upper;
            Value = value;
            NodeColour = nodeColour;
            Parent = parent;
        }

        public BalancedBoundingNode<T> Minimum()
        {
            var retVal = this;
            while (retVal.LowerTree != null)
            {
                retVal = retVal.LowerTree;
            }

            return retVal;
        }

        public BalancedBoundingNode<T> Maximum()
        {
            var retVal = this;
            while (retVal.UpperTree != null)
            {
                retVal = retVal.UpperTree;
            }

            return this;
        }

        public BalancedBoundingNode<T> Search(double value)
        {
            return Search(this, value);
        }

        public static BalancedBoundingNode<T> Search(BalancedBoundingNode<T> tree, double key)
        {
            while (tree != null && !(key >= tree.LowerBound && key < tree.UpperBound))
            {
                tree = key < tree.LowerBound ? tree.LowerTree : tree.UpperTree;
            }

            return tree;
        }

        public void WalkTree(Action<BalancedBoundingNode<T>> action)
        {
            if (action == null) return;

            LowerTree?.WalkTree(action);
            action(this);
            UpperTree?.WalkTree(action);
        }

        public void Insert(double lower, double upper, T value)
        {
            if (lower > upper) throw new ArgumentOutOfRangeException(nameof(lower), $"lower ({lower}) needs to be <= upper ({upper})");
            if (lower >= LowerBound && upper <= UpperBound)
            {
                throw new ArgumentOutOfRangeException($"Lower {lower} and upper {upper} overlap with this tree l:{LowerBound},u:{UpperBound}");
            }

            if (upper <= LowerBound)
            {
                if (LowerTree==null)
                {
                    LowerTree = new BalancedBoundingNode<T>(lower, upper, value, Colour.Black, this);
                }
                else
                {
                    LowerTree.Insert(lower, upper, value);
                }
            }
            else if (lower >= UpperBound)
            {
                if (UpperTree == null)
                {
                    UpperTree = new BalancedBoundingNode<T>(lower, upper, value, Colour.Black, this);
                }
                else
                {
                    UpperTree.Insert(lower, upper, value);
                }
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"BBT(L:{LowerBound},U:{UpperBound})";
        }

        public void LeftRotate()
        {
            var y = UpperTree;
            LowerTree = y.UpperTree;
            if (y.LowerTree != null)
            {
                y.LowerTree.Parent = this;
            }

            y.Parent = this.Parent;
            if (this.Parent != null)
            {
                if (this == this.Parent.LowerTree)
                {
                    this.Parent.LowerTree = y;
                }
                else
                {
                    this.Parent.UpperTree = y;
                }
            }

            y.LowerTree = this;
            this.Parent = y;
        }
    }
}
