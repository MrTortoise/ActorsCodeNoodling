using System;

namespace Entities.DataStructures
{
    /// <summary>
    /// Integer version of vector 2 ... this assumes all kinds of things about maths. 
    /// </summary>
    public struct Point2Int : IEquatable<Point2Int>
    {
        public static readonly Point2Int Zero = new Point2Int(0, 0);
        public static readonly Point2Int Max = new Point2Int(int.MaxValue,int.MaxValue);
        public static readonly Point2Int Min = new Point2Int(int.MinValue,int.MinValue);

        public Point2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Y { get; private set; }

        public int X { get; private set; }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return $"Point2Int({X},{Y}";
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. 
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point2Int && Equals((Point2Int) obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Point2Int other)
        {
            return Y == other.Y && X == other.X;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Y*397) ^ X;
            }
        }

        public static bool operator ==(Point2Int left, Point2Int right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point2Int left, Point2Int right)
        {
            return !left.Equals(right);
        }
    }
}