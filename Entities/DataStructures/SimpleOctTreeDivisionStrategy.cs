namespace Entities.DataStructures
{
    public class SimpleOctTreeDivisionStrategy<T> : ISimpleOctTreeDivisionStrategy<T>
    {
        public void SubDivide(Point3Int<T>[] points,
            Point3Int<T> point,
            BoundingCuboid boundary,
            out OctTree<T> tlf,
            out OctTree<T> trf,
            out OctTree<T> tlb,
            out OctTree<T> trb,
            out OctTree<T> blf,
            out OctTree<T> brf,
            out OctTree<T> blb,
            out OctTree<T> brb)
        {
            int maxItems = points.Length;
            var center = boundary.CenterPoint();

            //blf
            var minBlf = boundary.LowerLeft;
            var mzxBlf = center;
            blf = new OctTree<T>(new BoundingCuboid(minBlf, mzxBlf), maxItems, this);

            //brf
            var minBrf = new Point3Int(center.X, boundary.LowerLeft.Y, boundary.LowerLeft.Z);
            var maxBrf = new Point3Int(boundary.UpperRight.X, center.Y, center.Z);
            brf = new OctTree<T>(new BoundingCuboid(minBrf, maxBrf), maxItems, this);

            //blb
            var minBlb = new Point3Int(boundary.LowerLeft.X, boundary.LowerLeft.Y, center.Z);
            var maxBlb = new Point3Int(center.X, center.Y, boundary.UpperRight.Z);
            blb = new OctTree<T>(new BoundingCuboid(minBlb, maxBlb), maxItems, this);

            //brb
            var minBrb = new Point3Int(center.X, boundary.LowerLeft.Y, center.Z);
            var maxBrb = new Point3Int(boundary.UpperRight.X, center.Y, boundary.UpperRight.Z);
            brb = new OctTree<T>(new BoundingCuboid(minBrb, maxBrb), maxItems, this);

            //tlf
            var minTlf = new Point3Int(boundary.LowerLeft.X, center.Y, boundary.LowerLeft.Z);
            var maxTlf = new Point3Int(center.X, boundary.UpperRight.Y, center.Z);
            tlf = new OctTree<T>(new BoundingCuboid(minTlf, maxTlf), maxItems, this);

            //trf
            var minTrf = new Point3Int(center.X, center.Y, boundary.LowerLeft.Z);
            var maxTrf = new Point3Int(boundary.UpperRight.X, boundary.UpperRight.Y, center.Z);
            trf = new OctTree<T>(new BoundingCuboid(minTrf, maxTrf), maxItems, this);

            //tlb
            var minTlb = new Point3Int(boundary.LowerLeft.X, center.Y, center.Z);
            var maxTlb = new Point3Int(center.X, boundary.UpperRight.Y, boundary.UpperRight.Z);
            tlb = new OctTree<T>(new BoundingCuboid(minTlb, maxTlb), maxItems, this);

            //trb 
            var minTrb = center;
            var maxTrb = boundary.UpperRight;
            trb = new OctTree<T>(new BoundingCuboid(minTrb, maxTrb), maxItems, this);

            foreach (var point3Int in points)
            {
                AddPoint(blf, brf, blb, brb, tlf, trf, tlb, trb, point3Int, center);
            }

            AddPoint(blf, brf, blb, brb, tlf, trf, tlb, trb, point, center);
        }

        private void AddPoint(OctTree<T> blf, OctTree<T> brf, OctTree<T> blb, OctTree<T> brb, OctTree<T> tlf, OctTree<T> trf, OctTree<T> tlb, OctTree<T> trb, Point3Int<T> point, Point3Int center)
        {
            // is on right side
            var point3Int = point.Point;
            if (point3Int.X > center.X)
            {
                // is on top side
                if (point3Int.Y > center.Y)
                {
                    // is on the backside
                    if (point3Int.Z > center.Z)
                    {
                        trb.Add(point);
                    }
                    else
                    {
                        trf.Add(point);
                    }
                }
                else // is on bottom side
                {
                    // is on the backside
                    if (point3Int.Z > center.Z)
                    {
                        brb.Add(point);
                    }
                    else
                    {
                        brf.Add(point);
                    }
                }
            }
            else // is on left side
            {
                // is on top side
                if (point3Int.Y > center.Y)
                {
                    // is on the backside
                    if (point3Int.Z > center.Z)
                    {
                        tlb.Add(point);
                    }
                    else
                    {
                        tlf.Add(point);
                    }
                }
                else // is on bottom side
                {
                    // is on the backside
                    if (point3Int.Z > center.Z)
                    {
                        blb.Add(point);
                    }
                    else
                    {
                        blf.Add(point);
                    }
                }
            }
        }
    }
}