using System.Collections.Generic;

namespace Entities.DataStructures
{
    public class OctTree
    {
        public BoundingCuboid Boundary { get;  }
        public int MaxItems { get;  }
        public ISimpleOctTreeDivisionStrategy SimpleOctTreeDivisionStrategy { get;  }
        public Point3Int[] Points { get; private set; }

        public OctTree ULF { get; private set; }
        public OctTree URF { get; private set; }
        public OctTree ULB { get; private set; }
        public OctTree URB { get; private set; }
        public OctTree BLF { get; private set; }
        public OctTree BRF { get; private set; }
        public OctTree BLB { get; private set; }
        public OctTree BRB { get; private set; }

        public OctTree(BoundingCuboid boundary, int maxItems, ISimpleOctTreeDivisionStrategy simpleOctTreeDivisionStrategy)
        {
            Boundary = boundary;
            MaxItems = maxItems;
            SimpleOctTreeDivisionStrategy = simpleOctTreeDivisionStrategy;
            Points = new Point3Int[0];
        }

        public void Add(Point3Int point)
        {
            if (Points.Length + 1 > MaxItems)
            {
                SpawnChildrenAndSubdivide(point);
                return;
            }

            var newPoints = new Point3Int[Points.Length +1];
            Points.CopyTo(newPoints, 0);
            Points = newPoints;
            Points[Points.Length - 1] = point;
        }

        private void SpawnChildrenAndSubdivide(Point3Int point)
        {
            OctTree tlf;
            OctTree trf;
            OctTree tlb;
            OctTree trb;
            OctTree blf;
            OctTree brf;
            OctTree blb;
            OctTree brb;

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

        public void GetPointsWithinBoundary(BoundingCuboid area, ref List<Point3Int> points)
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

        private void GetLocalPointsInCuboid(BoundingCuboid cuboid, ref List<Point3Int> points)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                if (cuboid.ContainsPoint(Points[i]))
                {
                    points.Add(Points[i]);
                }
            }
        }
    }
}