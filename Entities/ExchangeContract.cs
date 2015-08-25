using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using Akka.Actor;

namespace Entities
{
    public class ExchangeContract : TypedActor,
        IHandle<ExchangeContract.PostInvitationMessage>,
        IHandle<ExchangeContract.QueryStateMessage>,
        IHandle<ExchangeContract.QueryOwner>,
        IHandle<ExchangeContract.QueryInvitationToTreat>,
        IHandle<ExchangeContract.PostOffer>,
        IHandle<ExchangeContract.QueryOffers>,
        IHandle<ExchangeContract.RejectOffer>

    {
        /// <summary>
        ///    The states of the ExchangeContractActor
        /// </summary>
        public enum State
        {
            Uninitialised,
            InvitationPosted,
            OfferRecieved,
            Rejected,
            CounterOffered
        }


        private State _state;
        private IActorRef _owner;
        private InvitationToTreat _invitationToTreat;
        private Offer _offer;

      public ExchangeContract()
      {
         _state = State.Uninitialised; 
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
         var offer = new Offer(Sender, message.OfferResourceStack,message.LiabilityResourceStack);
         _offer = offer;
         _state = State.OfferRecieved;
         _owner.Tell(new OfferMade(offer));
      }

      public void Handle(QueryOffers message)
      { 
         Sender.Tell(_offer);
      }

       public void Handle(RejectOffer message)
       {
           if (!ReferenceEquals(Sender, _owner))
           {
               throw new InvalidOperationException($"Rejecting offer on Exchange contract where Sender:{Sender.Path} != owner{_owner.Path}");
           }

           if (message.Offer == null)
           {
               _state = State.Rejected;
               _offer.Offerer.Tell(message);
               _owner.Tell(new LiabilityReturnedMessage(_invitationToTreat.LiabilityStack));
               Context.Parent.Tell(message);
           }
           else
           {
               _state = State.CounterOffered;
               _offer.Offerer.Tell(message);
           }
       }

       public struct LiabilityReturnedMessage
       {
           public ResourceStack LiabilityStack { get;  }

           public LiabilityReturnedMessage(ResourceStack liabilityStack)
           {
               LiabilityStack = liabilityStack;
           }
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
         public ResourceStack OfferResourceStack { get;  }
          public ResourceStack LiabilityResourceStack { get;  }

          public PostOffer(ResourceStack offerResourceStack, ResourceStack liabilityResourceStack)
          {
              OfferResourceStack = offerResourceStack;
              LiabilityResourceStack = liabilityResourceStack;
          }
      }

        /// <summary>
        /// Represents an offer on a contract.
        /// </summary>
        public class Offer
        {
            public Offer(IActorRef offerer, ResourceStack resourceStack, ResourceStack liabilityStack)
            {
                OfferStatus = OfferStatus.Outstanding;
                Offerer = offerer;
                ResourceStack = resourceStack;
                LiabilityStack = liabilityStack;
            }

            public IActorRef Offerer { get; }
            public ResourceStack ResourceStack { get; }
            public ResourceStack LiabilityStack { get; }
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

      public struct RejectOffer
      {
         public ResourceStack Offer { get;  }
          public ResourceStack Liability { get;  }

            /// <summary>
            /// Rejects the offer but offers a counter offer
            /// </summary>
            /// <param name="offer"></param>
            /// <param name="liability"></param>
          public RejectOffer(ResourceStack offer, ResourceStack liability)
          {
              Offer = offer;
              Liability = liability;
          }
      }
   }        
}