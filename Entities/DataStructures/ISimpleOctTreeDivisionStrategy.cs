namespace Entities.DataStructures
{
    public interface ISimpleOctTreeDivisionStrategy<T>
    {
        void SubDivide(Point3Int<T>[] points,
            Point3Int<T> point,
            BoundingCuboid boundary,
            out OctTree<T> tlf,
            out OctTree<T> trf,
            out OctTree<T> tlb,
            out OctTree<T> trb,
            out OctTree<T> blf,
            out OctTree<T> brf,
            out OctTree<T> blb,
            out OctTree<T> brb);
    }
}