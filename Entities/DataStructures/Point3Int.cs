using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataStructures
{
    public class Point3Int<T>
    {
        public Point3Int(Point3Int point, T value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            Point = point;
            Value = value;
        }

        public Point3Int(int x, int y, int z, T value)
            : this(new Point3Int(x, y, z), value){}

        public Point3Int Point { get; }
        public T Value { get; }
    }

    public struct Point3Int : IEquatable<Point3Int>
    {
        public static readonly Point3Int Min = new Point3Int(int.MinValue, int.MinValue, int.MinValue);
        public static readonly Point3Int Max = new Point3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        public static readonly Point3Int Zero = new Point3Int(0, 0, 0);

        public Point3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }


        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return $"Point3Int(X:{X},Y:{Y},Z:{Z})";
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
            return obj is Point3Int && Equals((Point3Int) obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Point3Int other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
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
                var hashCode = X;
                hashCode = (hashCode*397) ^ Y;
                hashCode = (hashCode*397) ^ Z;
                return hashCode;
            }
        }

        public static bool operator ==(Point3Int left, Point3Int right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point3Int left, Point3Int right)
        {
            return !left.Equals(right);
        }
    }
}
