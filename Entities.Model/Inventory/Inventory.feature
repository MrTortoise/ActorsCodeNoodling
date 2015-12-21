Feature: Inventory
	In order to store resources
	As pretty much any actor that holds resources
	I want to be able to configure

Background: 
	Given I create a test actor system
	And I add the following resources to the Resource Manager
	| name  |
	| metal |
	| rock  |

Scenario: Create and configure a basic inventory with capacity
	When I have created the following inventory types
	| Name           | Capacity | CargoSize |
	| testInventory  | 100      | Small     |
	| testInventory2 | 100      | Medium    |
	| testInventory3 | 100      | Large     |
	Then I expect the following inventory types to exist
	| Name           | Capacity | CargoSize |
	| testInventory  | 100      | Small     |
	| testInventory2 | 100      | Medium    |
	| testInventory3 | 100      | Large     |

Scenario: Create an inventory Add resource and check exists
	# fuck this needs testing in the context of an actor

