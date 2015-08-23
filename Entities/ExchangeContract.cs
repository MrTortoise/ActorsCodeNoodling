using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;

namespace Entities
{
   public class ExchangeContract : TypedActor,
      IHandle<ExchangeContract.PostInvitationMessage>,
      IHandle<ExchangeContract.QueryStateMessage>,
      IHandle<ExchangeContract.QueryOwner>,
      IHandle<ExchangeContract.QueryInvitationToTreat>,
      IHandle<ExchangeContract.PostOffer> ,
      IHandle<ExchangeContract.QueryOffers>

   {
      /// <summary>
      ///    The states of the ExchangeContractActor
      /// </summary>
      public enum State
      {
         Uninitialised,
         InvitationPosted,
         OfferRecieved
      }        
          
      private State _state;
      private IActorRef _owner;
      private InvitationToTreat _invitationToTreat;
      private readonly Dictionary<IActorRef,Offer> _offers;

      public ExchangeContract()
      {
         _state = State.Uninitialised; 
         _offers = new Dictionary<IActorRef, Offer>();
      }

      public void Handle(PostInvitationMessage message)
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

      public void Handle(QueryInvitationToTreat message)
      {
         Sender.Tell(_invitationToTreat);
      }

      public void Handle(QueryOwner message)
      {
         Sender.Tell(_owner);
      }

      public void Handle(QueryStateMessage message)
      {
         Sender.Tell(_state);
      }

      public void Handle(PostOffer message)
      {
         var offer = new Offer(Sender, message.ResourceStack);
         _offers.Add(Sender, offer);
         _state = State.OfferRecieved;
         _owner.Tell(new OfferMade(offer));
      }

      public void Handle(QueryOffers message)
      {
         var offers = _offers.Values.ToImmutableArray();
         Sender.Tell(offers);
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

      public struct PostOffer
      {
         public ResourceStack ResourceStack { get;  }

         public PostOffer(ResourceStack resourceStack)
         {
            ResourceStack = resourceStack;  
         }
      }

      /// <summary>
      /// Represents an offer on a contract.
      /// </summary>
      public class Offer
      {                                     
         public Offer(IActorRef offerer, ResourceStack resourceStack)
         {
            OfferStatus = OfferStatus.Outstanding;
            Offerer = offerer;
            ResourceStack = resourceStack;
         }

         //public Offer(IActorRef offerer, ResourceStack resourceStack, OfferStatus offerStatus)
         //{
         //   Offerer = offerer;
         //   OfferStatus = offerStatus;
         //   ResourceStack = resourceStack;
         //}

         public IActorRef Offerer { get;  }
         public ResourceStack ResourceStack { get; }
         public OfferStatus OfferStatus { get; }   
      }

      public enum OfferStatus
      {
         Outstanding,
         Declined,
         Accepted
      }

      public struct OfferMade
      {
         public Offer Offer { get; } 

         public OfferMade(Offer offer)
         {
            Offer = offer;
         }
      }

      public struct QueryOffers
      {
      }


   }        
}