Feature: Market
	In order to create a world for things to trade in
	As a demigod
	I want to be able to create markets and populate them

@mytag
Scenario: Create a market with a given name	
	When I create the following markets
	| id | name  |
	| 0  | test1 |
	Then the result should be 120 on the screen
