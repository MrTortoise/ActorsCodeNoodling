using Akka.Actor;

namespace Entities
{
   public class ExchangeContract   : TypedActor, 
      IHandle<ExchangeContract.PostInvitationMessage>,
      IHandle<ExchangeContract.QueryStateMessage>,
      IHandle<ExchangeContract.QueryOwner>,
      IHandle<ExchangeContract.QueryDto>

   {
      private State _state;

      public class PostInvitationMessage
      { 
         public OfferType ExchangeType { get; }
         public int LiabilityQuantity { get; }
         public Resource LiabilityResource { get; }
         public Resource SellResource { get; }
         public int SellResourceQuantity { get; }
         public TimePeriodType SellResourceTimePeriod { get; }
         public int SellResourceTimePeriodQuantity { get; }
         public Resource SuggestedOfferResource { get; }
         public int SuggestedQuantity { get; }

         public PostInvitationMessage(OfferType exchangeType, Resource sellResource, int sellResourceQuantity,
            TimePeriodType sellResourceTimePeriod, int sellResourceTimePeriodQuantity, Resource suggestedOfferResource,
            int suggestedQuantity, Resource liabilityResource, int liabilityQuantity)
         {
            this.ExchangeType = exchangeType;
            this.SellResource = sellResource;
            this.SellResourceQuantity = sellResourceQuantity;
            this.SellResourceTimePeriod = sellResourceTimePeriod;
            this.SellResourceTimePeriodQuantity = sellResourceTimePeriodQuantity;
            this.SuggestedOfferResource = suggestedOfferResource;
            this.SuggestedQuantity = suggestedQuantity;
            this.LiabilityResource = liabilityResource;
            this.LiabilityQuantity = liabilityQuantity; 
         }
      }

      public enum State
      {
         Uninitialised
      }

      public struct QueryStateMessage
      {
      }

      public struct QueryOwner
      {
      }

      public struct QueryDto
      {
      }

      public ExchangeContract()
      {
         _state = State.Uninitialised;
      }

      public void Handle(PostInvitationMessage message)
      {
         throw new System.NotImplementedException();
      }

      public void Handle(QueryStateMessage message)
      {
         Sender.Tell(_state);
      }

      public void Handle(QueryOwner message)
      {
         throw new System.NotImplementedException();
      }

      public void Handle(QueryDto message)
      {
         throw new System.NotImplementedException();
      }
   }
}