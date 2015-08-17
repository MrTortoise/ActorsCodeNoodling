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
         public DateTime GetDateTime()
         {
            return DateTime.Now;
         }
      }

      public static DateTime Now()
      {
         return _dateTimeProducer.GetDateTime();
      }

      public static DateTime NowPlusPeriod(TimePeriodType periodType, int quantity)
      {
         return Now().AddTimePeriod(periodType, quantity);
      }

      public static DateTime AddTimePeriod(this DateTime dateTime, TimePeriodType periodType, int quantity)
      {
         DateTime retVal = dateTime;
         switch (periodType)
         {
            case TimePeriodType.Second:
            {
               retVal = dateTime.AddSeconds(quantity);
               break;
            }
            case TimePeriodType.Minute:
            {
               retVal = dateTime.AddMinutes(quantity);
               break;
            }
            case TimePeriodType.Hour:
            {
               retVal = dateTime.AddHours(quantity);
               break;
            }
            case TimePeriodType.Day:
               retVal = dateTime.AddDays(quantity);
               break;
         }

         return retVal;
      }
   }
}