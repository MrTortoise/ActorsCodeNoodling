Feature: WorldLocation
	In order to have places and be able to dynamically add them
	As a WorldGenerator
	I want to be able to create Locations

Background: 
	Given I create a test actor system

Scenario: Take a list of prefixes, store them and then restore them
	Given I create a WorldPrefixPersistanceActor Actor
	And I create a TestProbe called "worldWatcher"
	And I create the following prefixes in the WorldPrefixPersistanceActor Actor and store its state
	| prefix |
	| qwe    |
	| asd    |
	| zxc    |
	| wer    |
	| sdf    |
	| xcv    |
	| ert    |
	| dfg    |
	| cvb    |
	When I kill and restore the WorldPrefixPersistanceActor Actor
	Then I expect querying the WorldPrefixPersistanceActor with TestProbe "worldWatcher" to yield the following prefixes
	| prefix |
	| qwe    |
	| asd    |
	| zxc    |
	| wer    |
	| sdf    |
	| xcv    |
	| ert    |
	| dfg    |
	| cvb    |
