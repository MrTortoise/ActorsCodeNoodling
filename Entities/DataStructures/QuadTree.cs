using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities.DataStructures
{
    public class QuadTree
    {
        public int MaxItems { get; private set; }
        public Bounding2DBox BoundingBox { get; private set; }
        public Point2Int[] Points { get; private set; }

        public IQuadTreeDivisionStrategy DivisionStrategy { get; private set; }

        public QuadTree NorthWest { get; private set; }
        public QuadTree NorthEast { get; private set; }
        public QuadTree SouthWest { get; private set; }
        public QuadTree SouthEast { get; private set; }

        public QuadTree(Bounding2DBox boundingBox, int maxItems, IQuadTreeDivisionStrategy divisionStrategy)
        {
            if (divisionStrategy == null) throw new ArgumentNullException(nameof(divisionStrategy));

            MaxItems = maxItems;
            DivisionStrategy = divisionStrategy;
            BoundingBox = boundingBox;
            Points = new Point2Int[0];
        }

        public void Add(Point2Int point)
        {
            if (!BoundingBox.ContainsPoint(point))
            {
                throw new ArgumentOutOfRangeException($"point:{point} is not inside bounding box:{BoundingBox}");
            }

            // invariant: if points == null then the children are active
            if (Points == null)
            {
                AddPointToChildren(point);
                return;
            }

            var nextIndex = Points.Length;
            if (nextIndex + 1 > MaxItems)
            {
                SpawnChildrenAndSubdivide(point);
                return;
            }

            var newPoints = new Point2Int[Points.Length + 1];
            Points.CopyTo(newPoints,0);
            Points = newPoints;
            Points[Points.Length - 1] = point;
        }

        private void SpawnChildrenAndSubdivide(Point2Int point)
        {
            QuadTree northWest;
            QuadTree northEast;
            QuadTree southWest;
            QuadTree southEast;

            DivisionStrategy.SubDivide(Points, point, BoundingBox, out northWest, out northEast, out southWest, out southEast);

            NorthWest = northWest;
            NorthEast = northEast;
            SouthWest = southWest;
            SouthEast = southEast;

            Points = null;
        }

        private void AddPointToChildren(Point2Int point)
        {
            var center = BoundingBox.CenterPoint();
            if (point.X < center.X)
            {
                if (point.Y < center.Y)
                {
                    SouthWest.Add(point);
                    return;
                }
                NorthWest.Add(point);
                return;
            }

            if (point.Y < center.Y)
            {
                SouthEast.Add(point);
                return;
            }
            NorthEast.Add(point);
        }

        /// <summary>
        /// Checks to see if the actual point is in the quad tree as opposed to being bounded by it
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool Contains(Point2Int point)
        {
            if (!BoundingBox.ContainsPoint(point))
            {
                return false;
            }

            if (Points != null)
            {
                return Points.Contains(point);
            }

            var center = BoundingBox.CenterPoint();
            if (point.X < center.X)
            {
                if (point.Y < center.Y)
                {
                    // bottom left
                    return SouthWest.Contains(point);
                }
                
                //top left
                return NorthWest.Contains(point);
            }

            if (point.Y < center.Y)
            {
                // bottom right
                return SouthEast.Contains(point);
            }
            //top right
            return NorthEast.Contains(point);
        }

        public void GetPointsInArea(Bounding2DBox area, ref List<Point2Int> list)
        {
            if (!BoundingBox.DoesBoundaryBoxIntersect(area))
            {
                return;
            }

            if (Points != null)
            {
                GetLocalPointsInArea(area, ref list);
            }
            else
            {
                NorthEast.GetPointsInArea(area, ref list);
                NorthWest.GetPointsInArea(area, ref list);
                SouthWest.GetPointsInArea(area, ref list);
                SouthEast.GetPointsInArea(area, ref list);
            }
        }

        private void GetLocalPointsInArea(Bounding2DBox area, ref List<Point2Int> list)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                if (area.ContainsPoint(Points[i]))
                {
                    list.Add(Points[i]);
                }
            }
        }
    }
}