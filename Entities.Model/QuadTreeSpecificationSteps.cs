using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using Entities.DataStructures;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    public class QuadTreeSpecificationSteps
    {
        private QuadTree _quadTree;

        [Given(@"I have created a QuadTree with min and max boundaries for x and y and max items per cell of (.*)")]
        public void GivenIHaveCreatedAQuadTreeWithMinAndMaxBoundariesForXAndYAndMaxItemsPerCellOf(int maxItems)
        {
            _quadTree = new QuadTree(new Bounding2DBox(new Point2Int(int.MinValue, int.MinValue), new Point2Int(int.MaxValue, int.MaxValue)), maxItems, new SimpleQuadTreeDivisionStrategy());
        }


        [When(@"I add a point to the Quadtree at (.*),(.*)")]
        public void WhenIAddAPointToTheQuadtreeAt(int x, int y)
        {
            var point = new Point2Int(x, y);
            _quadTree.Add(point);
        }
        
        [Then(@"I expect the QuadTree to contain the following points")]
        public void ThenIExpectTheQuadTreeToContainTheFollowingPoints(Table table)
        {
            var points = table.GetPoints();

            foreach (var vector2Int in points)
            {
                var isContained = _quadTree.Contains(vector2Int);
                Assert.IsTrue(isContained);
            }
        }
    }

}
