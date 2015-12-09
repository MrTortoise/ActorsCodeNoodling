Feature: FactoryCoordinatorActor
	In order to perform bulk operations on factories
	As a user
	I want to be able to add factories and factory types

Background: 
	Given I create a test actor system
	And I create a Resource Manager
	And I add the following resources to the Resource Manager
	| name        |
	| Metal       |
	| Rock        |
	| Punk        |
	| Hydrogen    |
	| geddit yet? |
	And I have created the following Material called "The Moon"
	| ResourceName | Value |
	| Metal        | 0.1   |
	| Rock         | 0.9   |
	And I have created the following Material called "Some Planet"
	| ResourceName | Value |
	| Metal        | 0.1   |
	| Rock         | 0.9   |
	And I have created the following Material called "Mellow Yellow"
	| ResourceName | Value |
	| Hydrogen     | 0.1   |
	And I have created the following Celestial Bodies         
	| name           | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | bodyType | material      | satellites                |
	| The Moon       | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Moon     | The Moon      |                           |
	| Another Moon   | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Moon     | The Moon      |                           |
	| The Planet     | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Planet   | Some Planet   | "The Moon","Another Moon" |
	| Other Planet   | 10     | 300           | 0.1                    | 0                           | 0                                   | 0                      | Planet   | Some Planet   |                           |
	| The Sun        | 1      | 2             | 0.2                    | 0                           | 0                                   | 0                      | Star     | Mellow Yellow |                           |
	| The Second Sun | 1      | 2             | 0.2                    | 0                           | 3.14                                | 3.14                   | Star     | Mellow Yellow |                           |
	And I create a CenterOfMassManagerActor
	And I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments
	| name         | stars                      | planets                     |
	| Solar System | "The Sun","The Second Sun" | "The Planet","Other Planet" |

Scenario: Add factory types and query them
	Given I have created a Factory Type called "FuckPhysics" with the following properties
	| In    | resource | quantity | periods |
	| false | metal    | 10       | 1       |
	When I query the factory types and store result in context as "FactoryTypes"
	Then I expect the factory type "FuckPhysics" with the following properties in context "FactoryTypes"
	| In    | resource | quantity | periods |
	| false | metal    | 10       | 1      |

Scenario: Create a factory
	Given I have created a Factory Type called "FuckPhysics" with the following properties
	| In    | resource | quantity | periods |
	| false | metal    | 10       | 1       |
	And I have created a Trader called "factoryCreator"
	When I create the following Factories using actor "factoryCreator"
	| name                        | factoryType | centerOfMass | celestialBody |
	| somethingFromNothingFactory | fuckPhysics | Solar System | Other Planet  |
	Then I expect the FactoryCoordinator to contain the following factories
	| name                        | factoryType | centerOfMass | celestialBody |
	| somethingFromNothingFactory | fuckPhysics | Solar System | Other Planet  |
	Then I expect the results of querying the trader "factoryCreator" for its factories to be
	| name                        | factoryType | centerOfMass | celestialBody |
	| somethingFromNothingFactory | fuckPhysics | Solar System | Other Planet  |