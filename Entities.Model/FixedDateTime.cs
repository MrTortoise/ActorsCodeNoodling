using System;

namespace Entities.Model
{
   /// <summary>
   /// DateTime producer that always returns the same datetime.
   /// </summary>
   public class FixedDateTime : IProduceDateTime
   {
      private readonly DateTime _dateTime; 

      public FixedDateTime(DateTime dateTime)
      {
         _dateTime = dateTime;
      }

      public DateTime GetDateTime()
      {
         return _dateTime;
      }

      public DateTime GetDateTime(TimePeriodType periodType, int quantity)
      {
         return _dateTime.AddTimePeriod(periodType, quantity);
      }
   }
}