Feature: FactoryCoordinatorActor
	In order to perform bulk operations on factories
	As a user
	I want to be able to add factories and factory types

Background: 
	Given I create a test actor system	
	And I have configured the heartBeat actor to update with the following configuration
	| updatePeriod | factoryUpdatePeriod |
	| 100          | 1000                |
	And I create the heirachy of coordinators
	And I add the following resources to the Resource Manager
	| name        |
	| Metal       |
	| Rock        |
	| Punk        |
	| Hydrogen    |
	| geddit yet? |
	And I have created the following Material called "The Moon"
	| ResourceName | Value |
	| Metal        | 1     |
	| Rock         | 9     |
	And I have created the following Material called "Some Planet"
	| ResourceName | Value |
	| Metal        | 1     |
	| Rock         | 9     |
	And I have created the following Material called "Mellow Yellow"
	| ResourceName | Value |
	| Hydrogen     | 1     |
	And I have created the following Celestial Bodies         
	| name           | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | bodyType | material      | satellites                |
	| The Moon       | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Moon     | The Moon      |                           |
	| Another Moon   | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Moon     | The Moon      |                           |
	| The Planet     | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Planet   | Some Planet   | "The Moon","Another Moon" |
	| Other Planet   | 10     | 300           | 0.1                    | 0                           | 0                                   | 0                      | Planet   | Some Planet   |                           |
	| The Sun        | 1      | 2             | 0.2                    | 0                           | 0                                   | 0                      | Star     | Mellow Yellow |                           |
	| The Second Sun | 1      | 2             | 0.2                    | 0                           | 3.14                                | 3.14                   | Star     | Mellow Yellow |                           |
	And I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments
	| name         | stars                      | planets                     |
	| Solar System | "The Sun","The Second Sun" | "The Planet","Other Planet" |
	And I have created the following inventory types
	| Name                  | Capacity | CargoSize |
	| smallFactoryInventory | 1000     | Small     |

Scenario: Add factory types and query them
	Given I have created a Factory Type called "FuckPhysics" with the following properties
	| In    | resource | quantity | periods | 
	| false | Metal    | 10       | 1       | 
	When I query the factory types and store result in context as "FactoryTypes"
	Then I expect the factory type "FuckPhysics" with the following properties in context "FactoryTypes"
	| In    | resource | quantity | periods | 
	| false | Metal    | 10       | 1       | 

Scenario: Create a factory
	Given I have created a Factory Type called "FuckPhysics" with the following properties
	| In    | resource | quantity | periods |
	| false | Metal    | 10       | 1       |
	And I have created a Trader called "factoryCreator"
	When I create the following Factories using actor "factoryCreator"
	| name                        | factoryType | centerOfMass | celestialBody | inventoryType         |
	| somethingFromNothingFactory | FuckPhysics | Solar System | Other Planet  | smallFactoryInventory |
	Then I expect the FactoryCoordinator to contain the following factories
	| name                        | factoryType | centerOfMass | celestialBody | inventoryType         |
	| somethingFromNothingFactory | FuckPhysics | Solar System | Other Planet  | smallFactoryInventory |
	Then I expect the results of querying the trader "factoryCreator" for its factories to be
	| name                        | factoryType | centerOfMass | celestialBody | inventoryType         |
	| somethingFromNothingFactory | FuckPhysics | Solar System | Other Planet  | smallFactoryInventory |