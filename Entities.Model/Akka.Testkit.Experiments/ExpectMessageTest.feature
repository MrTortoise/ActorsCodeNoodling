Feature: ExpectMessageTest
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Create Actor, make it send a message and assert it was sent
	Given I have created a relay actor
	And I have entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen
