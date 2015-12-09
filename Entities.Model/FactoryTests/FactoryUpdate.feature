Feature: FactoryUpdate
	In order to produce resources
	As a Factory
	I want to be able to update and produce stuff

	Background: 
	Given I create a test actor system
	And I create a Resource Manager
	And I add the following resources to the Resource Manager
	| name  |
	| metal |
	| rock  |
	And I set the FactoryCoordinator time period to "00:00:01"
	And I have created a FactoryCoordinator actor

Scenario: Setup a dummy actor, register with FactoryCoordinator can assert that updates are called when expected
	Given I create a TestProbe called "factory"
	And I add register the actor "factory" with FactoryCoordinator	
	When I wait for 3 FactoryCoordinator time periods
	Then I expect test probe "factory" will of received 3 update events

Scenario: Create a factory giv eit resources update and ensure has produced
	
	Given I have created a factory Type called "FuckPhysics" with the following properties
	| In    | resource | quantity | periods |
	| false | metal    | 10       | 1       |
	And I have created the following factory
	| name        | factoryType |
	| testFactory | FuckPhysics |	
	When I wait for 4 FactoryCoordinator time periods
	Then I expect the factory "testFactory" to have the followign resources
	| resource | quantity |
	| metal    | 40       |
