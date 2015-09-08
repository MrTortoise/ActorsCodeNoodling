Feature: WorldLocation
	In order to have places and be able to dynamically add them
	As a WorldGenerator
	I want to be able to create Locations


Scenario: Generate 10,000,000 names and verify they are unique
	Given I generate 10000000 location names
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
