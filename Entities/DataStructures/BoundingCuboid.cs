using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.ProtocolBuffers;

namespace Entities.DataStructures
{
    public struct BoundingCuboid : IEquatable<BoundingCuboid>
    {
        public static readonly BoundingCuboid Max = new BoundingCuboid(Point3Int.Min, Point3Int.Max);

        /// <summary>
        /// This is the corner that would be -ve,-ve,-ve
        /// </summary>
        public Point3Int LowerLeft { get; }
        /// <summary>
        /// in absence of a sensible name this is the opposite corner to the bottom left
        /// </summary>
        public Point3Int UpperRight { get; }

        public BoundingCuboid(Point3Int lowerLeft, Point3Int upperRight)
        {
            if (lowerLeft.X > upperRight.X
                || lowerLeft.Y > upperRight.Y
                || lowerLeft.Z > upperRight.Z)
            {
                throw new ArgumentOutOfRangeException($"lower left is not less than upper right. lowerLeft:{lowerLeft}, upperRight:{upperRight}");
            }

            LowerLeft = lowerLeft;
            UpperRight = upperRight;
        }

        public bool ContainsPoint(Point3Int point)
        {
            return point.X >= LowerLeft.X && point.X < UpperRight.X
                   && point.Y >= LowerLeft.Y && point.Y < UpperRight.Y
                   && point.Z >= LowerLeft.Z && point.Z < UpperRight.Z;
        }

        public bool DoesBoundaryCuboidIntersect(BoundingCuboid cuboid)
        {
            return cuboid.LowerLeft.X < this.UpperRight.X
                   && cuboid.LowerLeft.Y < this.UpperRight.Y
                   && cuboid.LowerLeft.Z < this.UpperRight.Z
                   && cuboid.UpperRight.X >= this.LowerLeft.X
                   && cuboid.UpperRight.Y >= this.LowerLeft.Y
                   && cuboid.UpperRight.Z >= this.LowerLeft.Z;
        }

        public Point3Int CenterPoint()
        {
            var midx = (LowerLeft.X + UpperRight.X)/2;
            var midy = (LowerLeft.Y + UpperRight.Y)/2;
            var midz = (LowerLeft.Z + UpperRight.Z)/2;

            return new Point3Int(midx, midy, midz);
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return $"BoundingCuboid(LL:{LowerLeft},UR:{UpperRight})";
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
            return obj is BoundingCuboid && Equals((BoundingCuboid) obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(BoundingCuboid other)
        {
            return LowerLeft.Equals(other.LowerLeft) && UpperRight.Equals(other.UpperRight);
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
                return (LowerLeft.GetHashCode()*397) ^ UpperRight.GetHashCode();
            }
        }

        public static bool operator ==(BoundingCuboid left, BoundingCuboid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BoundingCuboid left, BoundingCuboid right)
        {
            return !left.Equals(right);
        }

    }
}
