namespace Entities.DataStructures
{
    public class SimpleQuadTreeDivisionStrategy : IQuadTreeDivisionStrategy
    {
        public void SubDivide(Point2Int[] points, Point2Int newPoint, Bounding2DBox boundingBox, out QuadTree northWest, out QuadTree northEast, out QuadTree southWest, out QuadTree southEast)
        {
            int maxItems = points.Length;
            var center = boundingBox.CenterPoint();

            var bottomLeftOfNorthWest = new Point2Int(boundingBox.BottomLeft.X, center.Y);
            var topRightOfNorthWest = new Point2Int(center.X, boundingBox.TopRight.Y);
            var bottomLeftOfNorthEast = center;
            var topRightOfNorthEast = boundingBox.TopRight;
            var bottomLeftOfSouthWest = boundingBox.BottomLeft;
            var topRightOfSouthWest = center;
            var bottomLeftOfSouthEast = new Point2Int(center.X, boundingBox.BottomLeft.Y);
            var topRightOfSouthEast = new Point2Int(boundingBox.TopRight.X, center.Y);

            northWest = new QuadTree(new Bounding2DBox(bottomLeftOfNorthWest, topRightOfNorthWest), maxItems, this);
            northEast = new QuadTree(new Bounding2DBox(bottomLeftOfNorthEast, topRightOfNorthEast), maxItems, this);
            southWest = new QuadTree(new Bounding2DBox(bottomLeftOfSouthWest, topRightOfSouthWest), maxItems, this);
            southEast = new QuadTree(new Bounding2DBox(bottomLeftOfSouthEast, topRightOfSouthEast), maxItems, this);

            foreach (var point2Int in points)
            {
                AddPoint(northWest, northEast, southWest, southEast, point2Int, center);
            }

            AddPoint(northWest, northEast, southWest, southEast, newPoint, center);
        }

        /// <summary>
        /// Adds the point into the corrent quad
        /// </summary>
        /// <param name="northWest"></param>
        /// <param name="northEast"></param>
        /// <param name="southWest"></param>
        /// <param name="southEast"></param>
        /// <param name="point2Int"></param>
        /// <param name="center"></param>
        private static void AddPoint(QuadTree northWest, QuadTree northEast, QuadTree southWest, QuadTree southEast, Point2Int point2Int, Point2Int center)
        {
            // is point on east side
            if (point2Int.X > center.X)
            {
                // Is point on north
                if (point2Int.Y > center.Y)
                {
                    northEast.Add(point2Int);
                }
                else
                {
                    southEast.Add(point2Int);
                }
            }
            else
            {
                // Is point on north
                if (point2Int.Y > center.Y)
                {
                    northWest.Add(point2Int);
                }
                else
                {
                    southWest.Add(point2Int);
                }
            }
        }
    }
}