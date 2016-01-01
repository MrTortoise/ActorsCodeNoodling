using System;

namespace Entities.DataStructures
{
    /// <summary>
    /// This has been done as if 0,0 is bottom left. So that will be fun later.
    /// </summary>
    public struct Bounding2DBox : IEquatable<Bounding2DBox>
    {
        public static readonly Bounding2DBox Empty = new Bounding2DBox(Point2Int.Zero, Point2Int.Zero);
        public static readonly Bounding2DBox Max = new Bounding2DBox(Point2Int.Min, Point2Int.Max);

        public Point2Int BottomLeft { get; private set; }
        public Point2Int TopRight { get; private set; }

        public Bounding2DBox(Point2Int bottomLeft, Point2Int topRight)
        {
            if (bottomLeft.X > topRight.X || bottomLeft.Y > topRight.Y)
                throw new ArgumentOutOfRangeException($"Bottom Left is not bottom left of top right, bottomLeft:{bottomLeft}, topRight:{topRight}");

            BottomLeft = bottomLeft;
            TopRight = topRight;
        }

        /// <summary>
        /// Finds the middle of the bounding box
        /// </summary>
        /// <returns>the value will be rounded down so we use less than or equal to</returns>
        public Point2Int CenterPoint()
        {
            var middleX = (BottomLeft.X + TopRight.X)/2;
            var middleY = (BottomLeft.Y + TopRight.Y)/2;

            return new Point2Int(middleX, middleY);
        }

        /// <summary>
        /// Returns whether a given point lies inside the bounding box
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool ContainsPoint(Point2Int point)
        {
            return point.X < TopRight.X
                   && point.X >= BottomLeft.X
                   && point.Y < TopRight.Y
                   && point.Y >= BottomLeft.Y;
        }

        public bool DoesBoundaryBoxIntersect(Bounding2DBox area)
        {
            return area.BottomLeft.X < this.TopRight.X
                   && area.BottomLeft.Y < this.TopRight.Y
                   && area.TopRight.X >= this.BottomLeft.X
                   && area.TopRight.Y >= this.BottomLeft.Y;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return $"BoundingBox(BottomLeft:{BottomLeft},TopRight:{TopRight}";
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
            return obj is Bounding2DBox && Equals((Bounding2DBox) obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Bounding2DBox other)
        {
            return BottomLeft.Equals(other.BottomLeft) && TopRight.Equals(other.TopRight);
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
                return (BottomLeft.GetHashCode()*397) ^ TopRight.GetHashCode();
            }
        }

        public static bool operator ==(Bounding2DBox left, Bounding2DBox right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Bounding2DBox left, Bounding2DBox right)
        {
            return !left.Equals(right);
        }
    }
}