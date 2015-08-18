using System;
using Akka.Actor;

namespace Entities
{
   public class ExchangeContract : TypedActor,
      IHandle<ExchangeContract.PostInvitationMessage>,
      IHandle<ExchangeContract.QueryStateMessage>,
      IHandle<ExchangeContract.QueryOwner>,
      IHandle<ExchangeContract.QueryInvitationToTreat>

   {
      /// <summary>
      ///    The states of the ExchangeContractActor
      /// </summary>
      public enum State
      {
         Uninitialised,
         InvitationPosted
      }        
          
      private State _state;
      private IActorRef _owner;
      private InvitationToTreat _invitationToTreat;

      public ExchangeContract()
      {
         _state = State.Uninitialised; 
      }

      public void Handle(ExchangeContract.PostInvitationMessage message)
      {
         _invitationToTreat = new InvitationToTreat(
            message.ExchangeType,
            DateTimeProvider.NowPlusPeriod(message.InvitationResourceTimePeriod,
               message.InvitationResourceTimePeriodQuantity),
            new ResourceStack(message.InvitationResource, message.InvitationResourceQuantity),
            new ResourceStack(message.LiabilityResource, message.LiabilityQuantity),
            new ResourceStack(message.SuggestedOfferResource, message.SuggestedQuantity));

         _state = State.InvitationPosted;
         _owner = Sender;
      }

      public void Handle(ExchangeContract.QueryInvitationToTreat message)
      {
         Sender.Tell(_invitationToTreat);
      }

      public void Handle(ExchangeContract.QueryOwner message)
      {
         Sender.Tell(_owner);
      }

      public void Handle(ExchangeContract.QueryStateMessage message)
      {
         Sender.Tell(_state);
      }

      public class PostInvitationMessage
      {
         public PostInvitationMessage(OfferType exchangeType, Resource invitationResource,
            int invitationResourceQuantity,
            TimePeriodType invitationResourceTimePeriod, int invitationResourceTimePeriodQuantity,
            Resource suggestedOfferResource,
            int suggestedQuantity, Resource liabilityResource, int liabilityQuantity)
         {
            ExchangeType = exchangeType;

            InvitationResource = invitationResource;
            InvitationResourceQuantity = invitationResourceQuantity;
            InvitationResourceTimePeriod = invitationResourceTimePeriod;
            InvitationResourceTimePeriodQuantity = invitationResourceTimePeriodQuantity;

            SuggestedOfferResource = suggestedOfferResource;
            SuggestedQuantity = suggestedQuantity;

            LiabilityResource = liabilityResource;
            LiabilityQuantity = liabilityQuantity;
         }

         public OfferType ExchangeType { get; }
         public int LiabilityQuantity { get; }
         public Resource LiabilityResource { get; }
         public Resource InvitationResource { get; }
         public int InvitationResourceQuantity { get; }
         public TimePeriodType InvitationResourceTimePeriod { get; }
         public int InvitationResourceTimePeriodQuantity { get; }
         public Resource SuggestedOfferResource { get; }
         public int SuggestedQuantity { get; }
      }

      public struct QueryStateMessage
      {
      }

      public struct QueryOwner
      {
      }

      public struct QueryInvitationToTreat
      {
      }
   }
}