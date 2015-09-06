Feature: Mine
	In order to be able to distribute my seed
	As a mine
	I want to be able to produce a resource, let other actors query what i produce and how much of it i have.


Scenario: Create a Mine that produces a resource and query what resource it produces.
	When I have created a producer called "testMine" that produces the following resources
	| ConsumerType | Name  | Quantity | TimePeriodType | TimePeriodQuantity |
	| Produce      | metal | 10       | Second         | 5                  |
	Then I expect the producer called "testMine" produces the following resources
	| ConsumerType | Name  | Quantity | TimePeriodType | TimePeriodQuantity |
	| Produce      | metal | 10       | Second         | 5                  |

Scenario: Create a mine, wait for a time period	and confirm it has produced what is expected
	Given I have created a producer called "testMine" that produces the following resources
	| ConsumerType | Name  | Quantity | TimePeriodType | TimePeriodQuantity |
	| Produce      | metal | 10       | Second         | 5                  |
	When I wait for the following time period
	| TimePeriodType | Quantity |
	| Second         | 12       |
	Then I expect the producer called "testMine" to have the following resources available
	| ResourceName | Quantity |
	| Metal        | 20       |      
