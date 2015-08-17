using System;
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
      private OfferType _exchangeType;
      private ResourceStack _invitationStack;
      private DateTime _invitationDeadline;
      private ResourceStack _liabilityStack;
      private ResourceStack _suggestedOfferStack;
      private ActorPath _owner;

      public class PostInvitationMessage
      { 
         public OfferType ExchangeType { get; } 

         public int LiabilityQuantity { get; }
         public Resource LiabilityResource { get; } 

         public Resource InvitationResource { get; }
         public int InvitationResourceQuantity { get; }
         public TimePeriodType InvitationResourceTimePeriod { get; }
         public int InvitationResourceTimePeriodQuantity { get; } 

         public Resource SuggestedOfferResource { get; }
         public int SuggestedQuantity { get; }

         public PostInvitationMessage(OfferType exchangeType, Resource invitationResource, int invitationResourceQuantity,
            TimePeriodType invitationResourceTimePeriod, int invitationResourceTimePeriodQuantity, Resource suggestedOfferResource,
            int suggestedQuantity, Resource liabilityResource, int liabilityQuantity)
         {
            this.ExchangeType = exchangeType;

            this.InvitationResource = invitationResource;
            this.InvitationResourceQuantity = invitationResourceQuantity;
            this.InvitationResourceTimePeriod = invitationResourceTimePeriod;
            this.InvitationResourceTimePeriodQuantity = invitationResourceTimePeriodQuantity;

            this.SuggestedOfferResource = suggestedOfferResource;
            this.SuggestedQuantity = suggestedQuantity;

            this.LiabilityResource = liabilityResource;
            this.LiabilityQuantity = liabilityQuantity; 
         }
      }

      /// <summary>
      /// The states of the ExchangeContractActor
      /// </summary>
      public enum State
      {
         Uninitialised,
         InvitationPosted
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
         _exchangeType = message.ExchangeType;

         _invitationStack = new ResourceStack(message.InvitationResource, message.InvitationResourceQuantity);
         _invitationDeadline = DateTimeProvider.NowPlusPeriod(message.InvitationResourceTimePeriod,
            message.InvitationResourceTimePeriodQuantity);

         _liabilityStack = new ResourceStack(message.LiabilityResource, message.LiabilityQuantity);
         _suggestedOfferStack = new ResourceStack(message.SuggestedOfferResource, message.SuggestedQuantity);  

         _state = State.InvitationPosted;

         _owner = Sender.Path;
      }

      public void Handle(QueryStateMessage message)
      {
         Sender.Tell(_state);
      }

      public void Handle(QueryOwner message)
      {
        Sender.Tell(_owner);
      }

      public void Handle(QueryDto message)
      {
         throw new System.NotImplementedException();
      }
   }
}