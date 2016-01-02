using System.Collections.Generic;

namespace Entities.DataStructures
{
    public class OctTree<T>
    {
        public BoundingCuboid Boundary { get; }
        public int MaxItems { get; }
        public ISimpleOctTreeDivisionStrategy<T> SimpleOctTreeDivisionStrategy { get; }
        public Point3Int<T>[] Points { get; private set; }

        public OctTree<T> ULF { get; private set; }
        public OctTree<T> URF { get; private set; }
        public OctTree<T> ULB { get; private set; }
        public OctTree<T> URB { get; private set; }
        public OctTree<T> BLF { get; private set; }
        public OctTree<T> BRF { get; private set; }
        public OctTree<T> BLB { get; private set; }
        public OctTree<T> BRB { get; private set; }

        public OctTree(BoundingCuboid boundary, int maxItems, ISimpleOctTreeDivisionStrategy<T> simpleOctTreeDivisionStrategy)
        {
            Boundary = boundary;
            MaxItems = maxItems;
            SimpleOctTreeDivisionStrategy = simpleOctTreeDivisionStrategy;
            Points = new Point3Int<T>[0];
        }

        public void Add(Point3Int<T> point)
        {
            if (Points.Length + 1 > MaxItems)
            {
                SpawnChildrenAndSubdivide(point);
                return;
            }

            var newPoints = new Point3Int<T>[Points.Length + 1];
            Points.CopyTo(newPoints, 0);
            Points = newPoints;
            Points[Points.Length - 1] = point;
        }

        private void SpawnChildrenAndSubdivide(Point3Int<T> point)
        {
            OctTree<T> tlf;
            OctTree<T> trf;
            OctTree<T> tlb;
            OctTree<T> trb;
            OctTree<T> blf;
            OctTree<T> brf;
            OctTree<T> blb;
            OctTree<T> brb;

            SimpleOctTreeDivisionStrategy.SubDivide(Points, point, Boundary, out tlf, out trf, out tlb, out trb, out blf, out brf, out blb, out brb);

            ULF = tlf;
            URF = trf;
            URB = trb;
            ULB = tlb;
            BLF = blf;
            BRF = brf;
            BLB = blb;
            BRB = brb;

            Points = null;
        }

        public void GetPointsWithinBoundary(BoundingCuboid area, ref List<Point3Int<T>> points)
        {
            if (!Boundary.DoesBoundaryCuboidIntersect(area))
            {
                return;
            }

            if (Points != null)
            {
                GetLocalPointsInCuboid(area, ref points);
            }
            else
            {
                ULF.GetPointsWithinBoundary(area, ref points);
                URF.GetPointsWithinBoundary(area, ref points);
                URB.GetPointsWithinBoundary(area, ref points);
                ULB.GetPointsWithinBoundary(area, ref points);
                BLF.GetPointsWithinBoundary(area, ref points);
                BRF.GetPointsWithinBoundary(area, ref points);
                BLB.GetPointsWithinBoundary(area, ref points);
                BRB.GetPointsWithinBoundary(area, ref points);
            }
        }

        private void GetLocalPointsInCuboid(BoundingCuboid cuboid, ref List<Point3Int<T>> points)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                if (cuboid.ContainsPoint(Points[i].Point))
                {
                    points.Add(Points[i]);
                }
            }
        }
    }
}