Feature: FactoryUpdate
	In order to produce resources
	As a Factory
	I want to be able to update and produce stuff

	Background: 
	Given I create a test actor system
	And I have configured the heartBeat actor to update with the following configuration	
	| updatePeriod | factoryUpdatePeriod |
	| 100          | 1000                |
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
	And I have created a Factory Type called "FuckPhysics" with the following properties
	| In    | resource | quantity | periods |
	| false | Metal    | 10       | 1       |
	And I have created a Factory Type called "Consumer" with the following properties
	| In    | resource | quantity | periods |
	| true  | Rock     | 10       | 1       |
	| false | Metal    | 10       | 1       |
	And I have created a Trader called "factoryCreator"
	And I have created the following inventory types
	| Name                  | Capacity | CargoSize |
	| smallFactoryInventory | 1000     | Small     |
	And I tell the heartbeat actor to start

Scenario: Setup a dummy actor, register with FactoryCoordinator can assert that updates are called when expected
	Given I create the following Factories using actor "factoryCreator"
	| name                        | factoryType | centerOfMass | celestialBody | inventoryType         |
	| somethingFromNothingFactory | FuckPhysics | Solar System | Other Planet  | smallFactoryInventory |
	When I wait for 2 FactoryUpdate time periods
	Then I expect the factory "somethingFromNothingFactory" to have the following resources
	| ResourceName | Value |
	| Metal        | 30    |

Scenario: Update a factory that consumes and produces without sufficient input - assert update fails
	Given I create the following Factories using actor "factoryCreator"
		| name           | factoryType | centerOfMass | celestialBody | inventoryType         |
		| creatorOfMetal | Consumer    | Solar System | Other Planet  | smallFactoryInventory |
	And I deposit into the factory "creatorOfMetal" the following resources
		| ResourceName | Value |
		| Rock         | 0     |
	When I wait for 2 FactoryUpdate time periods	
	Then I expect the factory "creatorOfMetal" to have the following resources
		| ResourceName | Value |
		| Metal        | 0     |
		| Rock         | 0     |

Scenario: Update a factory that consumes and produces with sufficient input - assert update succeeds
	Given I create the following Factories using actor "factoryCreator"
		| name           | factoryType | centerOfMass | celestialBody | inventoryType         |
		| creatorOfMetal | Consumer    | Solar System | Other Planet  | smallFactoryInventory |
	And I deposit into the factory "creatorOfMetal" the following resources
		| ResourceName | Value |
		| Rock         | 30    |
	When I wait for 2 FactoryUpdate time periods	
	Then I expect the factory "creatorOfMetal" to have the following resources
		| ResourceName | Value |
		| Metal        | 30    |
		| Rock         | 0     |

Scenario: Update a factory that consumes and produces with constrained input - assert update succeeds correct number of times
	Given I create the following Factories using actor "factoryCreator"
		| name           | factoryType | centerOfMass | celestialBody | inventoryType         |
		| creatorOfMetal | Consumer    | Solar System | Other Planet  | smallFactoryInventory |
	And I deposit into the factory "creatorOfMetal" the following resources
		| ResourceName | Value |
		| Rock         | 20    |
	When I wait for 2 FactoryUpdate time periods	
	Then I expect the factory "creatorOfMetal" to have the following resources
		| ResourceName | Value |
		| Metal        | 20    |
		| Rock         | 0     |

