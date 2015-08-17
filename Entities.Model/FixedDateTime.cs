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

      public DateTime GetdateTime()
      {
         return _dateTime;
      }
   }
}