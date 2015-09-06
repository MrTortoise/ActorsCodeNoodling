Feature: MarketHub
	In order to create and manage markets for thiungs to trade stuff on
	As a demigod
	I want to be able to create markets, list them and query

Background: 
	Given I create a test actor system
	And I initialise the MarketHub Actor
	And I create a TestProbe called "marketCreator"

Scenario: Create a market in the listing and confirm its reference is as expected
	 When I create the following markets using testProbe "marketCreator"
	 | Name  |
	 | test1 |
	 Then I expect testProbe "marketCreator" of been notified of the market having been created
	 | Name  |
	 | test1 |

Scenario: Create a market with a given name	in the market listing
	When I create the following markets using testProbe "marketCreator"
	 | Name  |
	 | test1 |
	 | test2 |
	Then I expect to see the following markets when I query the listings
	| Name  |
	| test1 |
	| test2 |