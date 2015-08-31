@Autofackery
Feature: InstancePerScenario
	In order to do config per feature
	As a lazy dev
	I want to be able to recreate the container per scenario

Scenario:0 - Take an iterator, iterate it and confirm values start at 0
Given I configure, build and scope with the per scenario configuration
And I resolve an instance of the iterator and store it in the context
Then I expect the iterators value to be 0

Scenario:1 - Take an iterator, iterate it and confirm values start at 0 still
Given I configure, build and scope with the per scenario configuration
And I resolve an instance of the iterator and store it in the context
Then I expect the iterators value to be 0
When I iterate the iterators value
Then I expect the iterators value to be 1
When I iterate the iterators value
Then I expect the iterators value to be 2

Scenario:2 - Take an iterator, iterate it and confirm values still start at 0 still still
Given I configure, build and scope with the per scenario configuration
And I resolve an instance of the iterator and store it in the context
Then I expect the iterators value to be 0
When I iterate the iterators value
Then I expect the iterators value to be 1
When I iterate the iterators value
Then I expect the iterators value to be 2

