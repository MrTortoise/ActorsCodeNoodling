@actorSystem
Feature: Trader
	In order to profit
	As a Trader
	I want to be able to post invitations to trade, make offers, accept, reject and exchange goods

Scenario: Create a trader and give it some reources
	Given I have created a Trader called "Test"	
	When I post the folowing resources to the Trader "Test"
	| name  | quantity |
	| metal | 3        |
	| rock  | 4        |
	And I ask What resources Trader "Test" has storing them in the context as "TestResources"
	Then I expect the following resources in context "TestResources"
	| name  | quantity |
	| metal | 3        |
	| rock  | 4        |
