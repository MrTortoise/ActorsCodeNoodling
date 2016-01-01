Feature: QuadTreeSpecification
	In order to store a lot of non uniform points in a performant manner
	As a math idiot in a fully local universe
	I want to be able to add points adn knoe that i am using a data structure that is performant

@quadtree
Scenario: Create a quadtree Set its boundaries to max and min, add a point at 0,0
	Given I have created a QuadTree with min and max boundaries for x and y and max items per cell of 10
	When I add a point to the Quadtree at 0,0
	Then I expect the QuadTree to contain the following points
	| x | y |
	| 0 | 0 |
