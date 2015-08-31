Feature: ExchangeContractFeature
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Background: 
   Given I create a test actor system
   And I create a Resource Manager
   Given I add the following resources to the Resource Manager
	| name  |
	| metal |
	| rock  |
   And I have created a Trader called "seller"
   And I have created a Trader called "buyer"
   And I create a TestProbe called "buyer"
   And I create a TestProbe called "seller"
   And I create a TestProbe called "exchangeContractSup"
  # And I have created a TestActor called "testActor"
   And I have configured the DateTime provider to return "2015/1/1 15:00:00"

Scenario: Get an instance of ExchangeContractActor and verify it is uninitialised
	When I create an ExchangeContractActor called "exchangeContract" with supervisor TestProbe called "exchangeContractSup"
	Then I expect the state of the ExchangeContractActor "exchangeContract" to be "Uninitialised"

Scenario:  Post invitation to Exchange contract, verify its current state is posted and that it can be queried
	Given I create an ExchangeContractActor called "exchangeContract" with supervisor TestProbe called "exchangeContractSup"	
	When I post to the ExchangeContract "exchangeContract" the following invitation
	| Field                          | Value    |
	| ExchangeType                   | Purchase |
	| SellResourceName               | metal    |
	| SellResourceQuantity           | 10       |
	| SellResourceTimePeriod         | Hour     |
	| SellResourceTimePeriodQuantity | 1        |
	| SuggestedOfferResourceName     | rock     |
	| SuggestedOfferResourceQuantity | 2        |
	| LiabilityResourceName          | metal    |
	| LiabilityResourceQuantity      | 2        |
	| ContractOwner                  | seller     |
	Then I expect the state of the ExchangeContractActor "exchangeContract" to be "InvitationPosted"
    And I expect the creator of ExchangeContractActor "exchangeContract" to be "seller"
    And I expect the ExchangeContractActor "exchangeContract" to have the following for offer
    | Field                          | Value             |
    | ExchangeType                   | Purchase          |
    | Resource                       | metal             |
    | Quantity                       | 10                |
    | CompletionTime                 | 2015/1/1 16:00:00 |
    | SuggestedOfferResourceName     | rock              |
    | SuggestedOfferResourceQuantity | 2                 |
    | LiabilityResourceName          | metal             |
    | LiabilityResourceQuantity      | 2                 |

Scenario: Take an invitation and make an offer, verify state is OfferMade, offer is queryable, and owner gets notified
	Given I create an ExchangeContractActor called "exchangeContract" with supervisor TestProbe called "exchangeContractSup"		
	And I post to the ExchangeContract "exchangeContract" the following invitation
	| Field                          | Value     |
	| ExchangeType                   | Purchase  |
	| SellResourceName               | metal     |
	| SellResourceQuantity           | 10        |
	| SellResourceTimePeriod         | Hour      |
	| SellResourceTimePeriodQuantity | 1         |
	| SuggestedOfferResourceName     | rock      |
	| SuggestedOfferResourceQuantity | 2         |
	| LiabilityResourceName          | metal     |
	| LiabilityResourceQuantity      | 2         |
	| ContractOwner                  | testActor |
    When the Trader called "buyer" makes the following offer on the ExchangeContractActor called "exchangeContract"
    | Field             | Value |
    | Resource          | metal |
    | Quantity          | 8     |
    | LiabilityResource | metal |
    | LiabilityQuantity | 1     |
    Then I expect that the TestActor will of been notified of the following offer being made
    | Field             | Value |
    | Resource          | metal |
    | Quantity          | 8     |
    | LiabilityResource | metal |
    | LiabilityQuantity | 1     |
    | SenderName        | buyer |
    Then I expect the state of the ExchangeContractActor "exchangeContract" to be "OfferRecieved" 
    And I expect an offer on the ExchangeContractActor called "exchangeContract" to be
    | Field             | Value |
    | Resource          | metal |
    | Quantity          | 8     |
    | LiabilityResource | metal |
    | LiabilityQuantity | 1     |
    | SenderName        | buyer |

   Scenario: Take an invitation that is under offer, reject it, ensure owner gets liability back and supervisor of offer notified.
	Given I create an ExchangeContractActor called "exchangeContract" with supervisor TestProbe called "exchangeContractSup"	
	And I post to the ExchangeContract "exchangeContract" the following invitation using a TestProbe
	| Field                          | Value    |
	| ExchangeType                   | Purchase |
	| SellResourceName               | metal    |
	| SellResourceQuantity           | 10       |
	| SellResourceTimePeriod         | Hour     |
	| SellResourceTimePeriodQuantity | 1        |
	| SuggestedOfferResourceName     | rock     |
	| SuggestedOfferResourceQuantity | 2        |
	| LiabilityResourceName          | metal    |
	| LiabilityResourceQuantity      | 2        |
	| ContractOwner                  | seller   |
   And the TestProbe called "buyer" makes the following offer on the ExchangeContractActor called "exchangeContract"
   | Field             | Value |
   | Resource          | metal |
   | Quantity          | 8     |
   | LiabilityResource | metal |
   | LiabilityQuantity | 1     |
   When the TestProbe called "seller" rejects the offer on the ExchangeContractActor called "exchangeContract"
   Then I expect the TestProbe "exchangeContractSup" to of recieved the message Offer Rejected Notification
   And I expect that the TestProbe "buyer" will of recieved an empty Offer Rejected Message   
   And I expect the TestProbe "seller" to recieve the following Liability Message
   | Field    | Value |
   | Resource | metal |
   | Quantity | 2     |
   And I expect the TestProbe "buyer" to recieve the following Liability Message
   | Field    | Value |
   | Resource | metal |
   | Quantity | 1     |
   And I expect the state of the ExchangeContractActor "exchangeContract" to be "OfferRejected"
  
Scenario: Take an invitation that is under offer, reject it and make an alternate suggestion
	Given I create an ExchangeContractActor called "exchangeContract" with supervisor TestProbe called "exchangeContractSup"	
	And I post to the ExchangeContract "exchangeContract" the following invitation using a TestProbe
	| Field                          | Value    |
	| ExchangeType                   | Purchase |
	| SellResourceName               | metal    |
	| SellResourceQuantity           | 10       |
	| SellResourceTimePeriod         | Hour     |
	| SellResourceTimePeriodQuantity | 1        |
	| SuggestedOfferResourceName     | rock     |
	| SuggestedOfferResourceQuantity | 2        |
	| LiabilityResourceName          | metal    |
	| LiabilityResourceQuantity      | 2        |
	| ContractOwner                  | seller   |
   And the TestProbe called "buyer" makes the following offer on the ExchangeContractActor called "exchangeContract"
   | Field             | Value |
   | Resource          | metal |
   | Quantity          | 8     |
   | LiabilityResource | metal |
   | LiabilityQuantity | 1     |
   When the TestProbe called "seller" rejects the offer on the ExchangeContractActor called "exchangeContract" and makes the following suggested offer
   | Field             | Value |
   | Resource          | metal |
   | Quantity          | 10    |
   | LiabilityResource | metal |
   | LiabilityQuantity | 1     |
   Then I expect that the TestProbe "buyer" will of recieved the following suggested offer
   | Field             | Value |
   | Resource          | metal |
   | Quantity          | 10    |
   | LiabilityResource | metal |
   | LiabilityQuantity | 1     |
   And I expect the state of the ExchangeContractActor "exchangeContract" to be "CounterOffered"

Scenario: Take an invitation that is under offer, accept it. Need to start escrow.
	Given I create an ExchangeContractActor called "exchangeContract" with supervisor TestProbe called "exchangeContractSup"	
	And I post to the ExchangeContract "exchangeContract" the following invitation using a TestProbe
	| Field                          | Value    |
	| ExchangeType                   | Purchase |
	| SellResourceName               | metal    |
	| SellResourceQuantity           | 10       |
	| SellResourceTimePeriod         | Hour     |
	| SellResourceTimePeriodQuantity | 1        |
	| SuggestedOfferResourceName     | rock     |
	| SuggestedOfferResourceQuantity | 2        |
	| LiabilityResourceName          | metal    |
	| LiabilityResourceQuantity      | 2        |
	| ContractOwner                  | seller   |
   And the TestProbe called "buyer" makes the following offer on the ExchangeContractActor called "exchangeContract"
   | Field             | Value |
   | Resource          | metal |
   | Quantity          | 8     |
   | LiabilityResource | metal |
   | LiabilityQuantity | 1     |
   When the TestProbe called "seller" rejects the offer on the ExchangeContractActor called "exchangeContract"
   Then I expect the state of the ExchangeContractActor "exchangeContract" to be "OfferAccepted"


   
