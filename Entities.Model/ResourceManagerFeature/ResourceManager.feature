
Feature: ResourceManager
	In order to keep references to resources singular and immutable
	As an Actor
	I want to be able to add and return resource instances

@actorSystem
@resourceManager
Scenario: Given a resource manager from the context, add a resource and then return it by its name
	When I press add the following resources to the Resource Manager
	| name  |
	| metal |
	| rock  |
	Then the I expect the Resource  Manager to contain the following resources
	| name  |
	| metal |
	| rock  |
