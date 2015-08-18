using System;

namespace Entities
{
   public class InvitationToTreat
   {
      public OfferType ExchangeType { get; }
      public DateTime InvitationDeadline { get; }
      public ResourceStack InvitationStack { get; }  
      public ResourceStack LiabilityStack { get; }
      public ResourceStack SuggestedOffer { get; }

      public InvitationToTreat(
         OfferType exchangeType, 
         DateTime invitationDeadline, 
         ResourceStack invitationStack, 
         ResourceStack liabilityStack, 
         ResourceStack suggestedOffer)
      {
         ExchangeType = exchangeType;
         InvitationDeadline = invitationDeadline;
         InvitationStack = invitationStack;
         LiabilityStack = liabilityStack;
         SuggestedOffer = suggestedOffer;
      }  
   }
}