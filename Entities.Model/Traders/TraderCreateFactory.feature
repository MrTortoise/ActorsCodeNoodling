Feature: TraderCreateFactory
	In order to be able to produce resources
	As a trader
	I want to be able to build a factory to consume and produce resources

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
	And I have created the following Material called "Some Planet"
	| ResourceName | Value |
	| Metal        | 0.1   |
	| Rock         | 0.9   |
	And I have created the following Material called "Mellow Yellow"
	| ResourceName | Value |
	| Hydrogen     | 0.1   |
	And I have created the following Celestial Bodies         
	| name           | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | bodyType | material      | satellites                |
	| The Moon       | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Moon     | The Moon      |                           |
	| Another Moon   | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Moon     | The Moon      |                           |
	| The Planet     | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Planet   | Some Planet   | "The Moon","Another Moon" |
	| Other Planet   | 10     | 300           | 0.1                    | 0                           | 0                                   | 0                      | Planet   | Some Planet   |                           |
	| The Sun        | 1      | 2             | 0.2                    | 0                           | 0                                   | 0                      | Star     | Mellow Yellow |                           |
	| The Second Sun | 1      | 2             | 0.2                    | 0                           | 3.14                                | 3.14                   | Star     | Mellow Yellow |                           |
	And I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments
	| name         | stars                      | planets                     |
	| Solar System | "The Sun","The Second Sun" | "The Planet","Other Planet" |

Scenario: Take a center of mass actor and create a factory 
	Given I have created a Trader called "factoryCreator"
	When I create a factory
	Then the result should be 120 on the screen
