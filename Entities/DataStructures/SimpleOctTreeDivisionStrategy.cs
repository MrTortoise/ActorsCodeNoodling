namespace Entities.DataStructures
{
    public class SimpleOctTreeDivisionStrategy : ISimpleOctTreeDivisionStrategy
    {
        public void SubDivide(Point3Int[] points, Point3Int point, BoundingCuboid boundary, out OctTree tlf, out OctTree trf, out OctTree tlb, out OctTree trb, out OctTree blf, out OctTree brf, out OctTree blb, out OctTree brb)
        {
            int maxItems = points.Length;
            var center = boundary.CenterPoint();

            //blf
            var minBlf = boundary.LowerLeft;
            var mzxBlf = center;
            blf = new OctTree(new BoundingCuboid(minBlf, mzxBlf), maxItems, this);

            //brf
            var minBrf = new Point3Int(center.X, boundary.LowerLeft.Y, boundary.LowerLeft.Z);
            var maxBrf = new Point3Int(boundary.UpperRight.X, center.Y, center.Z);
            brf = new OctTree(new BoundingCuboid(minBrf, maxBrf), maxItems, this);

            //blb
            var minBlb = new Point3Int(boundary.LowerLeft.X, boundary.LowerLeft.Y, center.Z);
            var maxBlb = new Point3Int(center.X, center.Y, boundary.UpperRight.Z);
            blb = new OctTree(new BoundingCuboid(minBlb, maxBlb), maxItems, this);

            //brb
            var minBrb = new Point3Int(center.X, boundary.LowerLeft.Y, center.Z);
            var maxBrb = new Point3Int(boundary.UpperRight.X, center.Y, boundary.UpperRight.Z);
            brb = new OctTree(new BoundingCuboid(minBrb, maxBrb), maxItems, this);

            //tlf
            var minTlf = new Point3Int(boundary.LowerLeft.X, center.Y, boundary.LowerLeft.Z);
            var maxTlf = new Point3Int(center.X, boundary.UpperRight.Y, center.Z);
            tlf = new OctTree(new BoundingCuboid(minTlf, maxTlf), maxItems, this);

            //trf
            var minTrf = new Point3Int(center.X, center.Y, boundary.LowerLeft.Z);
            var maxTrf = new Point3Int(boundary.UpperRight.X, boundary.UpperRight.Y, center.Z);
            trf = new OctTree(new BoundingCuboid(minTrf, maxTrf), maxItems, this);

            //tlb
            var minTlb = new Point3Int(boundary.LowerLeft.X, center.Y, center.Z);
            var maxTlb = new Point3Int(center.X, boundary.UpperRight.Y, boundary.UpperRight.Z);
            tlb = new OctTree(new BoundingCuboid(minTlb, maxTlb), maxItems, this);

            //trb 
            var minTrb = center;
            var maxTrb = boundary.UpperRight;
            trb = new OctTree(new BoundingCuboid(minTrb, maxTrb), maxItems, this);

            foreach (var point3Int in points)
            {
                AddPoint(blf, brf, blb, brb, tlf, trf, tlb, trb, point3Int, center);
            }

            AddPoint(blf, brf, blb, brb, tlf, trf, tlb, trb, point, center);
        }

        private void AddPoint(OctTree blf, OctTree brf, OctTree blb, OctTree brb, OctTree tlf, OctTree trf, OctTree tlb, OctTree trb, Point3Int point, Point3Int center)
        {
            // is on right side
            if (point.X > center.X)
            {
                // is on top side
                if (point.Y > center.Y)
                {
                    // is on the backside
                    if (point.Z > center.Z)
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
                    if (point.Z > center.Z)
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
                if (point.Y > center.Y)
                {
                    // is on the backside
                    if (point.Z > center.Z)
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
                    if (point.Z > center.Z)
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