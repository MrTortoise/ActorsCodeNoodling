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

@actorSystem
@resourceManager
Scenario: Get an instance of ExchangeContractActor and verify it is uninitialised
	When I create an ExchangeContractActor called "test"
	Then I expect the state of the ExchangeContractActor "test" to be "Uninitialised"

@actorSystem
@resourceManager
Scenario:  Post invitation to Exchange contract and verify its current state is posted
	Given I create an ExchangeContractActor called "test"	
	And I have created a Trader called "Test"
   And I have configured the DateTime provider to return "2015/1/1 15:00:00"
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

   
