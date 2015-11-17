Feature: WorldLocationGenerator
	In order to have places to visit
	As a content builder
	I want to be able to add locations

@Persistence
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
	And I have cleared out any persistence sql data

@LocationGenerator
Scenario: Add a location
	Given I have created a LocationGenerator Actor
	And I create a TestProbe called "LocationWatcher"
	And I observe LocationGenerator with TestProbe "LocationWatcher"
	When I add a location using "LocationWatcher" called "test"
	Then I expect that TestProbe "LocationWatcher" be told the following locations was added "test"	
	When I poison the LocationGenerator
	And I have created a LocationGenerator Actor
	Then I expect the location "test" to exist
