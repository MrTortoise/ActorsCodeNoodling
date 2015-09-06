Feature: Market
	In order to create a world for things to trade in
	As a demigod
	I want to be able to create markets and populate them

Background: 
	Given I create a test actor system
	And I initialise the MarketListings Actor

Scenario: Create a market in the listing and confirm its reference is as expected
	 When I create the following markets
	 | Name  |
	 | test1 |
	 Then I expect the returned market to be 
	 | Name  |
	 | test1 |

Scenario: Create a market with a given name	in the market listing
	When I create the following markets
	 | Name  |
	 | test1 |
	 | test2 |
	And I query the market listings 
	Then I expect to see the following markets
	| Name  |
	| test1 |
	| test2 |
