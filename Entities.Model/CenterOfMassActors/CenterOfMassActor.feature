Feature: CenterOfMassActor
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers


Background: 
# The funny thing is that this is not a persistent actor as of yet. 
# I have no idea when it will be appropiate to call save state yet - every update seems excessive 
# as currently if we know the total delta from system start we can recreate so - perhaps - we dont need to store it
# that said over long periods of time we could overflow - there is no reason to suppose the entire system would have a natural harmonic ... or is rational or however you want to think it
# maybe that suggests we should represent angular velocities in terms of integer periods per timescale so that we can produce natural harmonics?
# do we care on placing an opper limit on maximum period of rotation? Eg its a game so a max rotational period of 1 week is a fucking long time.
# as the total timescale gets larger the more the risk of loss of accuracy due to datatypes .... so there is a sweet spot
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
	| name        |
	| metal       |
	| rock        |
	| punk        |
	| geddit yet? |
	And I have created the following Moon Type called "The Moon"
	| ResourceName | Value |
	| Metal        | 0.1   |
	| Rock         | 0.9   |
	And I have created the following moons
	| name         | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | moonType |
	| The Moon     | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | The Moon |
	| Another Moon | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | The Moon |
	And I have created the following Planet Type called "Some Planet"
	| ResourceName | Value |
	| Metal        | 0.1   |
	| Rock         | 0.9   |
	And I have created the following planets
	| name         | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | planetType  | moons                     |
	| The Planet   | 10     | 100           | 0.1                    | 0                           | 0                                   | 0                      | Some Planet | "The Moon","Another Moon" |
	| Other Planet | 10     | 300           | 0.1                    | 0                           | 0                                   | 0                      | Some Planet |                           |
	And I have created the following Star Type called "Mellow Yellow"
	| property | Value |
	| fuelRate | 0.1   |
	And I have created the following stars
	| name           | radius | orbitDistance | orbitalAngularVelocity | rotatationalAngularVelocity | initialOrbitalAngularPositionOffset | currentAngularPosition | starType      |
	| The Sun        | 1      | 2             | 0.2                    | 0                           | 0                                   | 0                      | Mellow Yellow |
	| The Second Sun | 1      | 2             | 0.2                    | 0                           | 3.14                                | 3.14                   | Mellow Yellow |


@mytag
Scenario: Create a center of mass actor, add a com with stars and planets etc and verify they exist
	Given I create a CenterOfMassManagerActor
	When I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments
	| name         | stars                      | planets                     |
	| Solar System | "The Sun","The Second Sun" | "The Planet","Other Planet" |
	Then the result should be 120 on the screen
