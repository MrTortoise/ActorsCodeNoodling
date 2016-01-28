
Feature: ResourceManager
	In order to keep references to resources singular and immutable
	As an Actor
	I want to be able to add and return resource instances


Background: 
   Given I create a test actor system

Scenario: Given a resource manager from the context, add a resource and then return it by its name
	When I add the following resources to the Resource Manager
	| name  |
	| metal |
	| rock  |
	Then the I expect the Resource  Manager to contain the following resources
	| name  |
	| metal |
	| rock  |

Scenario: Sign something up to observe resource manager and add a resource then i want to be notified
	Given I create a TestProbe called "resourceManagerMonitor"
	And I send an observe message to actor "ResourceManager" from actor "resourceManagerMonitor"
	When I add the following resources to the Resource Manager
	| name  |
	| metal |
	| rock  |
	Then I expect the TestProbe "resourceManagerMonitor" to recieve an event message
         
