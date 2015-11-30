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
	And I create a CenterOfMassManagerActor
	And I send messages of type CreateCenterOfMass to actor CenterOfMassManagerActor with arguments
	| name         | stars                      | planets                     |
	| Solar System | "The Sun","The Second Sun" | "The Planet","Other Planet" |


Scenario: Create a center of mass actor, add a com with stars and planets etc and verify they exist

	When I get the CenterOfMassActor called "Solar System" and store it in the context as "Solar System"
	Then I Expect the solar system "Solar System" to have the following
	| ObjectType | Name           |
	| star       | The Sun        |
	| star       | The Second Sun |
	| planet     | The Planet     |
	| planet     | Other Planet   |
	| moon       | The Moon       |
	| moon       | Another Moon   |

Scenario: Take the COM actor and do a few updates and ensure all is hunky dory.
# whilst this is goign on need to consider how to resolve the position of a body given the root of its coordinate space is in its parent and it has no reference to this atm.
# moreover due to the method of constructino passing a reference to a parent is a pain in the tits. Would ahve to switch from attaching things to parents creatign children 
# this has fairly large impacts upon things like order of creation, and so sequencing in storage and normalisation. Deserailising is not as simple as mass deserialisation followed by repointing.

