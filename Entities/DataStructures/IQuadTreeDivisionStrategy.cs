namespace Entities.DataStructures
{
    /// <summary>
    /// Responsible for subdividing a quad when it exceeds capacity
    /// </summary>
    public interface IQuadTreeDivisionStrategy
    {
        /// <summary>
        /// Takes existing points and new points, divide the area into 4 and then adeds points into it recursing until all areas have less than maximum number of points
        /// </summary>
        /// <param name="points"></param>
        /// <param name="newPoint"></param>
        /// <param name="boundingBox"></param>
        /// <param name="northWest"></param>
        /// <param name="northEast"></param>
        /// <param name="southWest"></param>
        /// <param name="southEast"></param>
        void SubDivide(Point2Int[] points, Point2Int newPoint, Bounding2DBox boundingBox, out QuadTree northWest, out QuadTree northEast, out QuadTree southWest, out QuadTree southEast);
    }
}
