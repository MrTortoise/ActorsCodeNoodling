using System;
using System.Security;

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

        [SecurityCritical]
        public DateTime GetDateTime()
        {
            return _dateTime;
        }
    }
}