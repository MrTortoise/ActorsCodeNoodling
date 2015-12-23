using TechTalk.SpecFlow;

namespace Entities.Model
{
    [Binding]
    public class SqlPersistenceSteps
    {
        [Given(@"I have cleared out any persistence sql data")]
        public void GivenIHaveClearedOutAnyPersistenceSqlData()
        {
            SqlPersistenceHelpers.ClearDatabase();
        }
    }
}