using System;
using System.CodeDom;
using System.Diagnostics;

namespace Entities.DataStructures
{
    /// <summary>
    /// Red-Black tree for nodes that are bounded by 2 values. EG a non continuous histogram.
    /// </summary>
    /// <remarks>This is all based on chapter 13 of introduction to algorithms.</remarks>
    /// <typeparam name="TBounds">The type used for the bounds checking</typeparam>
    /// <typeparam name="TPayload"></typeparam>
    public class BalancedBoundingTree<TBounds, TPayload> 
        where TBounds : IComparable<TBounds>, IEquatable<TBounds>
    {
        public static readonly BalancedBoundingNode Nil;

        static BalancedBoundingTree()
        {
            Nil = new BalancedBoundingNode(default(TBounds), default(TBounds), default(TPayload), BalancedBoundingNode.Colour.Black);
            Nil.LowerTree = Nil;
            Nil.UpperTree = Nil;
            Nil.Parent = Nil;
        }

        public BalancedBoundingTree(TBounds lower, TBounds upper, TPayload value)
        {
            Root = new BalancedBoundingNode(lower, upper, value, BalancedBoundingNode.Colour.Black);
        }

        public static BalancedBoundingNode Search(BalancedBoundingNode node, TBounds key)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            while (!ReferenceEquals(node, Nil) && !(key.CompareTo(node.LowerBound) >= 0 && key.CompareTo(node.UpperBound) < 0))
            {
                node = key.CompareTo(node.LowerBound) < 0 ? node.LowerTree : node.UpperTree;
            }

            return node;
        }

        public BalancedBoundingNode Root { get; set; }

        public BalancedBoundingNode Minimum()
        {
            var retVal = Root;
            while (!ReferenceEquals(retVal.LowerTree,Nil))
            {
                retVal = retVal.LowerTree;
            }

            return retVal;
        }

        public BalancedBoundingNode Maximum()
        {
            var retVal = Root;
            while (!ReferenceEquals(retVal.UpperTree,Nil))
            {
                retVal = retVal.UpperTree;
            }

            return retVal;
        }

        public BalancedBoundingNode Search(TBounds value)
        {
            return Search(Root, value);
        }

        public BalancedBoundingNode Insert(TBounds lower, TBounds upper, TPayload value)
        {
            var node = new BalancedBoundingNode(lower, upper, value, BalancedBoundingNode.Colour.Black);
            Insert(node);
            return node;
        }

        public void Insert(BalancedBoundingNode node)
        {
            BalancedBoundingNode y = Nil;
            BalancedBoundingNode x = Root;

            // find the parent of the node
            while (x != Nil)
            {
                y = x;
                if (node.CompareTo(x) < 0)
                {
                    x = x.LowerTree;
                }
                else
                {
                    x = x.UpperTree;
                }
            }
            node.Parent = y;

            if (y == Nil)
            {
                Root = node;
            }
            else if (node.CompareTo(y) < 0)
            {
                y.LowerTree = node;
            }
            else
            {
                y.UpperTree = node;
            }

            node.LowerTree = Nil;
            node.UpperTree = Nil;
            node.NodeColour = BalancedBoundingNode.Colour.Red;
            FixupNode(node);
        }

        public void FixupNode(BalancedBoundingNode node)
        {
            while (node.Parent.NodeColour == BalancedBoundingNode.Colour.Red)
            {
                if (node.Parent == node.Parent.Parent.LowerTree)
                {
                    var y = node.Parent.Parent.UpperTree;
                    if (y.NodeColour == BalancedBoundingNode.Colour.Red)
                    {
                        node.Parent.NodeColour = BalancedBoundingNode.Colour.Black;
                        y.NodeColour = BalancedBoundingNode.Colour.Black;
                        node.Parent.Parent.NodeColour = BalancedBoundingNode.Colour.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.UpperTree)
                        {
                            node = node.Parent;
                            LeftRotate(node);
                        }
                        node.Parent.NodeColour = BalancedBoundingNode.Colour.Black;
                        node.Parent.Parent.NodeColour = BalancedBoundingNode.Colour.Red;
                        RightRotate(node.Parent.Parent);
                    }
                }
                else
                {
                    var y = node.Parent.Parent.LowerTree;
                    if (y.NodeColour == BalancedBoundingNode.Colour.Red)
                    {
                        node.Parent.NodeColour = BalancedBoundingNode.Colour.Black;
                        y.NodeColour = BalancedBoundingNode.Colour.Black;
                        node.Parent.Parent.NodeColour = BalancedBoundingNode.Colour.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.LowerTree)
                        {
                            node = node.Parent;
                            RightRotate(node);
                        }
                        node.Parent.NodeColour = BalancedBoundingNode.Colour.Black;
                        node.Parent.Parent.NodeColour = BalancedBoundingNode.Colour.Red;
                        LeftRotate(node.Parent.Parent);
                    }
                }
            }
            Root.NodeColour = BalancedBoundingNode.Colour.Black;
        }

        public int CountNodes()
        {
            int total = 0;
            Root.WalkTree((n)=>total++);
            return total;
        }

        public void LeftRotate(BalancedBoundingNode x)
        {
            BalancedBoundingNode y = x.UpperTree; // set y
            x.UpperTree = y.LowerTree; // y used to take this position so safe to write to. 
            // now y.lower can be set.
            if (!ReferenceEquals(y.LowerTree, Nil)) // hook up the parent
            {
                y.LowerTree.Parent = x;
            }

            y.Parent = x.Parent; // x's old parent is now y parent (eg at the root node Nil)

            if (ReferenceEquals(x.Parent, Nil)) // so we need to figure out if x was root node, if so tree needs to know y is new root.
            {
                this.Root = y;
            }
            else if (ReferenceEquals(x, x.Parent.LowerTree)) // otherwise we need to figure out if x was lower or upper of its parent.
            {
                x.Parent.LowerTree = y;
            }
            else
            {
                x.Parent.UpperTree = y;
            }

            y.LowerTree = x;
            x.Parent = y;
        }

        public void RightRotate(BalancedBoundingNode y) 
        {
            BalancedBoundingNode x = y.LowerTree;
            y.LowerTree = x.UpperTree; // y lower is undefined not so set that
            if (x.UpperTree != Nil) // if the node that is being assigned is nto nil then set its parent also.
            {
                x.UpperTree.Parent = y;
            }

            x.Parent = y.Parent; // setup the parent of new root.
            if (y.Parent == Nil) // if the parent was nil then we know that it is the root node of the tree.
            {
                this.Root = x;
            }
            else if (y == y.Parent.LowerTree) // otherwise we need to fix up based on whatever side of the parent node y sits on
            {
                y.Parent.LowerTree = x;
            }
            else
            {
                y.Parent.UpperTree = x;
            }

            x.UpperTree = y;
            y.Parent = x;
        }

        /// <summary>
        /// Represents a node in the tree.
        /// </summary>
        public class BalancedBoundingNode : IEquatable<BalancedBoundingNode>, IComparable<BalancedBoundingNode>
        {
            public BalancedBoundingNode Parent { get;  set; }
            public BalancedBoundingNode LowerTree { get;  set; }
            public BalancedBoundingNode UpperTree { get;  set; }

            public TBounds LowerBound { get; }
            public TBounds UpperBound { get; }

            public TPayload Value { get; }

            public Colour NodeColour { get; set; }

            public enum Colour
            {
                Black,
                Red
            }

            public BalancedBoundingNode(TBounds lower, TBounds upper, TPayload value, Colour nodeColour)
            {
                if (lower.CompareTo(upper) > 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(lower), $"lower({lower}) must be <= upper({upper}");
                }

                LowerBound = lower;
                UpperBound = upper;
                Value = value;
                NodeColour = nodeColour;

                LowerTree = Nil;
                UpperTree = Nil;
                Parent = Nil;
            }

            public void WalkTree(Action<BalancedBoundingNode> action)
            {
                if (action == null) return;

                LowerTree?.WalkTree(action);
                action(this);
                UpperTree?.WalkTree(action);
            }

            public BalancedBoundingNode Insert(TBounds lower, TBounds upper, TPayload value)
            {
                if (lower.CompareTo(upper) > 0) throw new ArgumentOutOfRangeException(nameof(lower), $"lower ({lower}) needs to be <= upper ({upper})");

                if (upper.CompareTo(LowerBound) <= 0)
                {
                    if (ReferenceEquals(LowerTree, Nil))
                    {
                        LowerTree = new BalancedBoundingNode(lower, upper, value, Colour.Black) {Parent = this};
                        return LowerTree;
                    }
                    return LowerTree.Insert(lower, upper, value);
                }

                if (lower.CompareTo(UpperBound) >= 0)
                {
                    if (ReferenceEquals(UpperTree, Nil))
                    {
                        UpperTree = new BalancedBoundingNode(lower, upper, value, Colour.Black) {Parent = this};
                        return UpperTree;
                    }
                    return UpperTree.Insert(lower, upper, value);
                }

                throw new ArgumentOutOfRangeException($"Lower {lower} and upper {upper} overlap with this tree l:{LowerBound},u:{UpperBound}");
            }

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            [DebuggerStepThrough]
            public bool Equals(BalancedBoundingNode other)
            {
                if (other == null) return false;

                return ReferenceEquals(UpperTree, other.UpperTree)
                       && ReferenceEquals(LowerTree, other.LowerTree)
                       && ReferenceEquals(Parent, other.Parent)
                       && ReferenceEquals(Value, other.Value)
                       && LowerBound.Equals(other.LowerBound)
                       && UpperBound.Equals(other.UpperBound)
                       && NodeColour.Equals(other.NodeColour);
            }

            /// <summary>
            /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object. 
            /// </summary>
            /// <returns>
            /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other"/> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other"/>. Greater than zero This instance follows <paramref name="other"/> in the sort order. 
            /// </returns>
            /// <param name="other">An object to compare with this instance. </param>
            [DebuggerStepThrough]
            public int CompareTo(BalancedBoundingNode other)
            {
                if (other == null) throw new ArgumentNullException(nameof(other));

                if (this.Equals(other))
                {
                    return 0;
                }

                // test for lower than
                if (LowerBound.CompareTo(other.UpperBound) < 0)
                {
                    return -1;
                }

                if (UpperBound.CompareTo(other.LowerBound) > 0)
                {
                    return 1;
                }

                throw new ArgumentOutOfRangeException(nameof(other), "Other overlaps with btu does not equal this");
            }

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>
            /// A string that represents the current object.
            /// </returns>
            [DebuggerStepThrough]
            public override string ToString()
            {
                return $"BBT:{LowerBound}:{UpperBound}:{NodeColour})";
            }
        }
    }
}