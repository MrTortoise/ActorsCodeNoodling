﻿Feature: Heartbeat
	In order to tell actors to update beats
	As an actor system
	I want to be able to take a dateTime, compare it to last and then issue a number of update periods at a perticular frequency

Background: 
	Given I create a test actor system	

Scenario: setup a heartbeat, register an actor and start the system
	Given I have configured the heartBeat actor to update with the following configuration
	| updatePeriod | factoryUpdatePeriod |
	| 100          | 1000                |
	And I create a TestProbe called "heartBeatMonitor"
	And I register actor "heartBeatMonitor" with the HeartBeat actor as type "Tick"
	When I tell the heartbeat actor to start
	Then I expect the actor "heartBeatMonitor" to recieve the Start message

Scenario: setup a heartbeat, register an actor, start let run for several beats and ensure right number of beats get called.

	Given I have configured the heartBeat actor to update with the following configuration
	| updatePeriod | factoryUpdatePeriod |
	| 100          | 1000                |
	And I create a TestProbe called "heartBeatMonitor"
	And I register actor "heartBeatMonitor" with the HeartBeat actor as type "Tick"
	When I tell the heartbeat actor to start
	Then I expect the actor "heartBeatMonitor" to recieve the Start message
	When I wait for 3 TickUpdate time periods 
	Then I expect TestProbe "heartBeatMonitor" to receive at least 3 TickUpdate messages
