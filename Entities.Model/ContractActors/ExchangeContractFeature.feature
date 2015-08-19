Feature: ExchangeContractFeature
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@actorSystem
@resourceManager
Background: 
   Given I add the following resources to the Resource Manager
	| name  |
	| metal |
	| rock  |
   And I have created a Trader called "seller"
   And I have created a Trader called "buyer"
   And I have configured the DateTime provider to return "2015/1/1 15:00:00"

@actorSystem
@resourceManager
Scenario: Get an instance of ExchangeContractActor and verify it is uninitialised
	When I create an ExchangeContractActor called "test"
	Then I expect the state of the ExchangeContractActor "test" to be "Uninitialised"

@actorSystem
@resourceManager
Scenario:  Post invitation to Exchange contract, verify its current state is posted and that it can be queried
	Given I create an ExchangeContractActor called "test"	
	When I post to the ExchangeContract "test" the following invitation
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
	| ContractOwner                  | Test     |
	Then I expect the state of the ExchangeContractActor "test" to be "InvitationPosted"
   And I expect the creator of ExchangeContractActor "test" to be "Test"
   And I expect the ExchangeContractActor "test" to have the following for offer
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
	Given I create an ExchangeContractActor called "test"	
	And I post to the ExchangeContract "test" the following invitation
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
	| ContractOwner                  | Test     |
   When the Trader called "buyer" makes the following offer on the ExchangeContractActor called "test"
   | Field    | Value |
   | Resource | metal |
   | Quantity | 8     |
   Then I expect that the Trader "seller" will of been notified of an offer being made
   And I expect the offer on the ExchangeContractActor called "test" to be
   | Field    | Value |
   | Resource | metal |
   | Quantity | 8     |
   And I expect the state of the ExchangeContractActor "test" to be "Offered"

Scenario: Take an invitation that is under offer, reject it and make an alternate suggestion
	Given I create an ExchangeContractActor called "test"	
	And I post to the ExchangeContract "test" the following invitation
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
	| ContractOwner                  | Test     |
   And the Trader called "buyer" makes the following offer on the ExchangeContractActor called "test"
   | Field    | Value |
   | Resource | metal |
   | Quantity | 8     |
   When the Trader called "seller" rejects the offer on the ExchangeContractActor called "test" and makes the following suggested offer
   | Field    | Value |
   | Resource | metal |
   | Quantity | 10    |
   Then I expect that the Trader "seller" will of been notified of a suggested offer being made 
   And I expect the suggested offer on the ExchangeContractActor called "test" to be
   | Field    | Value |
   | Resource | metal |
   | Quantity | 10    |
   And I expect the state of the ExchangeContractActor "test" to be "CounterOffered"

Scenario: Take an invitation that is under offer, accept it 

// what is the difference between the suggested offer and the deposited liabilities.
// all are messages ... sp what about actors that misbehave?
// problem is after accepting offer we end up in escrow ... so why not just use ledgers from the start
// ie in a ledger an offer is placed with the liability to balance
   
