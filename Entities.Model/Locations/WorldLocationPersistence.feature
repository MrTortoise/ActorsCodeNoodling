@Persistence
@ignore
Feature: WorldLocationPersistence
	In order to have places and be able to dynamically add them
	As a WorldGenerator
	I want to be able to create Locations

Background: 
	Given I have cleared out any persistence file data
	Given I create a test actor system
	And I create a TestProbe called "worldWatcher"

@Persistence
Scenario: Take a list of prefixes, store them and then restore them
	Given I create a WorldPrefixPersistanceActor Actor using testProbe "worldWatcher"	
	And I create the following prefixes in the WorldPrefixPersistanceActor Actor and store its state using test probe "worldWatcher"
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

@Persistence
Scenario: Take a list of prefixes, store them, kill actor and then restore them
	Given I create a WorldPrefixPersistanceActor Actor using testProbe "worldWatcher"	
	And I create the following prefixes in the WorldPrefixPersistanceActor Actor and store its state using test probe "worldWatcher"
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
	When I kill the WorldPrefixPersistanceActor Actor
	And I create a WorldPrefixPersistanceActor Actor using testProbe "worldWatcher"	
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

