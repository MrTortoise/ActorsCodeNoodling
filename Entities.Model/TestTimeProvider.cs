using System;

namespace Entities.Model
{
    public class TestTimeProvider : IProduceDateTime
    {
        private DateTime _dateTime;

        public TestTimeProvider(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public void SetDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
        }

        public DateTime GetDateTime()
        {
            return _dateTime;
        }
    }
}