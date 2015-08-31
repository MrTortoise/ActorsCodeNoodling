@Autofackery
Feature: InstancePerFeaturePart2
	In order to share expensive configuration accross scenarios
	As a lazy dev
	I want to confirm the iterator resets to 0 and still iterates

Scenario:0 - Take the iterator, confirm is 0 and then iterate to a value 3
Given I configure, build and scope with the per feature configuration
And I resolve an instance of the iterator and store it in the context
Then I expect the iterators value to be 0
When I iterate the iterators value
Then I expect the iterators value to be 1
When I iterate the iterators value
Then I expect the iterators value to be 2
When I iterate the iterators value
Then I expect the iterators value to be 3

Scenario:1 - Take the iterator, confirm is 3 and then iterate to a value 6
Given I resolve an instance of the iterator and store it in the context
Then I expect the iterators value to be 3
When I iterate the iterators value
Then I expect the iterators value to be 4
When I iterate the iterators value
Then I expect the iterators value to be 5
When I iterate the iterators value
Then I expect the iterators value to be 6

Scenario:2 - Take the iterator, confirm is 6 and then iterate to a value 9
Given I resolve an instance of the iterator and store it in the context
Then I expect the iterators value to be 6
When I iterate the iterators value
Then I expect the iterators value to be 7
When I iterate the iterators value
Then I expect the iterators value to be 8
When I iterate the iterators value
Then I expect the iterators value to be 9
