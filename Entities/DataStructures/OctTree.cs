namespace Entities.DataStructures
{
    public class OctTree
    {
        public BoundingCuboid Boundary { get;  }
        public int MaxItems { get;  }
        public ISimpleOctTreeDivisionStrategy SimpleOctTreeDivisionStrategy { get;  }
        public Point3Int[] Points { get; private set; }

        public OctTree(BoundingCuboid boundary, int maxItems, ISimpleOctTreeDivisionStrategy simpleOctTreeDivisionStrategy)
        {
            Boundary = boundary;
            MaxItems = maxItems;
            SimpleOctTreeDivisionStrategy = simpleOctTreeDivisionStrategy;
        }

        public void Add(Point3Int point)
        {
            //if (Points.Length + 1 > MaxItems)
            //{
            //    SpawnChildrenAndSubdivide(point);
            //    return;
            //}

            var newPoints = new Point3Int[Points.Length +1];
            Points.CopyTo(newPoints, 0);
            Points = newPoints;
            Points[Points.Length - 1] = point;
        }
    }
}