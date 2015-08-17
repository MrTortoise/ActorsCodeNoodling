using System;

namespace Entities
{
   public static class DateTimeProvider
   {
      private static IProduceDateTime _dateTimeProducer = new DateTimeProducer();

      public static void SetProvider(IProduceDateTime dateTimeProducer)
      {
         _dateTimeProducer = dateTimeProducer;
      }

      public class DateTimeProducer : IProduceDateTime
      {
         public DateTime GetdateTime()
         {
            return DateTime.Now;
         }
      }
   }
}