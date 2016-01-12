Feature: DistributionGenerator
	In order to generate interesting shaped distributions nad save them
	As a universe editor
	I want to be able to create and store various distributions of numbers

@distribution
Scenario: Create a linear distribution of numbers between 2 extents
	Given I create a Distribution generator actor
	And I create the following equality function instances
	| name         | min | max |
	| testEquality | -10 | -10 |
	When I create the following distributions instance
	| name             | function     | noItems |
	| testDistribution | testEquality | 100     |
	Then the result should be 120 on the screen
