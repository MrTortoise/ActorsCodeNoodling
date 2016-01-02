namespace Entities.DataStructures
{
    public interface ISimpleOctTreeDivisionStrategy
    {
        void SubDivide(Point3Int[] points, Point3Int point, BoundingCuboid boundary, out OctTree tlf, out OctTree trf, out OctTree tlb, out OctTree trb, out OctTree blf, out OctTree brf, out OctTree blb, out OctTree brb);
    }
}