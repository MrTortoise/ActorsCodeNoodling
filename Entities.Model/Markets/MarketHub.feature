Feature: MarketHub
	In order to create and manage markets for thiungs to trade stuff on
	As a demigod
	I want to be able to create markets, list them and query

Background: 
Given I create a test actor system using config
	"""
	akka { 
	   loglevel=DEBUG,  loggers=["Akka.Logger.Serilog.SerilogLogger, Akka.Logger.Serilog"]
	      persistence {
                        publish-plugin-commands = on
                        journal {
                            plugin = "akka.persistence.journal.sql-server"
                            sql-server {
                                class = "Akka.Persistence.SqlServer.Journal.SqlServerJournal, Akka.Persistence.SqlServer"
                                plugin-dispatcher = "akka.actor.default-dispatcher"
                                table-name = EventJournal
                                schema-name = dbo
                                auto-initialize = on
                                connection-string = "Data Source=localhost\\SQLEXPRESS;Database=AkkaPersistenceTest;User Id=akkadotnet;Password=akkadotnet;"
                            }
                        }
                        snapshot-store {
                            plugin = "akka.persistence.snapshot-store.sql-server"
                            sql-server {
                                class = "Akka.Persistence.SqlServer.Snapshot.SqlServerSnapshotStore, Akka.Persistence.SqlServer"
                                plugin-dispatcher = "akka.actor.default-dispatcher"
                                table-name = SnapshotStore
                                schema-name = dbo
                                auto-initialize = on
                                connection-string = "Data Source=localhost\\SQLEXPRESS;Database=AkkaPersistenceTest;User Id=akkadotnet;Password=akkadotnet;"
                            }
                        }
                    }
		}
	"""
	And I create a Resource Manager
	And I add the following resources to the Resource Manager
	| name     |
	| Metal    |
	| Rock     |
	| Punk     |
	| Hydrogen |
	| geddit yet? |
	And I have created the following Material called "The Moon"
	| ResourceName | Value |
	| Metal        | 0.1   |
	| Rock         | 0.9   |
	And I have created the following moons
	| name         | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | moonType |
	| The Moon     | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | The Moon |
	| Another Moon | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | The Moon |
	And I have created the following Material called "Some Planet"
	| ResourceName | Value |
	| Metal        | 0.1   |
	| Rock         | 0.9   |
	And I have created the following planets
	| name         | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | planetType  | moons                     |
	| The Planet   | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Some Planet | "The Moon","Another Moon" |
	| Other Planet | 10     | 300           | 0.1                    | 0                           | 0                                   | 0                      | Some Planet |                           |
	And I have created the following Material called "Mellow Yellow"
	| ResourceName | Value |
	| Hydrogen     | 0.1   |
	And I have created the following stars
	| name           | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | starType      |
	| The Sun        | 1      | 2             | 0.2                    | 0                           | 0                                   | 0                      | Mellow Yellow |
	| The Second Sun | 1      | 2             | 0.2                    | 0                           | 3.14                                | 3.14                   | Mellow Yellow |
	And I create a CenterOfMassManagerActor
	And I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments
	| name         | stars                      | planets                     |
	| Solar System | "The Sun","The Second Sun" | "The Planet","Other Planet" |
	And I initialise the MarketHub Actor
	And I create a TestProbe called "marketCreator"	

Scenario: Create a market in the listing and confirm its reference is as expected
	 When I create the following markets using testProbe "marketCreator"
	 | Name  | Location   |
	 | test1 | Sopmewhere |
	 Then I expect testProbe "marketCreator" of been notified of the market having been created
	 | Name  | Location   |
	 | test1 | Sopmewhere |

Scenario: Create a market with a given name	in the market listing
	When I create the following markets using testProbe "marketCreator"
	 | Name  | Location      |
	 | test1 | Somewhere     |
	 | test2 | SomewhereElse |
	Then I expect to see the following markets when I query the listings
	| Name  | Location      |
	| test1 | Somewhere     |
	| test2 | SomewhereElse |