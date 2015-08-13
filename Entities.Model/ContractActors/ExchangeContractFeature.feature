Feature: ExchangeContractFeature
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@actorSystem
@resourceManager
Scenario: Get an instance of ExchangeContract Actor and verify it is uninitialised
	When I create an ExchangeContract Actor called "test"
	Then I expect the state of the Exchange contract actor "test" to be "Uninitialised"

Scenario:  Post invitation to Exchange contract and verify its current state is posted
	Given I create an ExchangeContract Actor called "test"	
	And I have created a Trader called "Test"
	When I post to the ExchangeContract "test" the following invitation
	| Field                          | Value |
	| SellResourceName               |       |
	| SellResourceQuantity           |       |
	| SellResourceTime               |       |
	| SuggestedOfferResourceName     |       |
	| SuggestedOfferResourceQuantity |       |
	| LiabilityResourceName          |       |
	| LiabilityResourceQuantity      |       |
	| TraderName                     |       |
	Then the result should be 120 on the screen
