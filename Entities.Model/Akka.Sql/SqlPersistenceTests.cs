﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using Akka.Persistence.SqlServer;
using Akka.TestKit.NUnit;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Entities.Model.Akka.Sql
{
    [TestFixture]
    class SqlPersistenceTests
    {

        private const string Config = @"
                    akka.persistence {
                        publish-plugin-commands = on
                        journal {
                            plugin = ""akka.persistence.journal.sql-server""
                            sql-server {
                                class = ""Akka.Persistence.SqlServer.Journal.SqlServerJournal, Akka.Persistence.SqlServer""
                                plugin-dispatcher = ""akka.actor.default-dispatcher""
                                table-name = EventJournal
                                schema-name = dbo
                                auto-initialize = on
                                connection-string = ""Data Source=localhost\\SQLEXPRESS;Database=AkkaPersistenceTest;User Id=akkadotnet;Password=akkadotnet;""
                            }
                        }
                        snapshot-store {
                            plugin = ""akka.persistence.snapshot-store.sql-server""
                            sql-server {
                                class = ""Akka.Persistence.SqlServer.Snapshot.SqlServerSnapshotStore, Akka.Persistence.SqlServer""
                                plugin-dispatcher = ""akka.actor.default-dispatcher""
                                table-name = SnapshotStore
                                schema-name = dbo
                                auto-initialize = on
                                connection-string = ""Data Source=localhost\\SQLEXPRESS;Database=AkkaPersistenceTest;User Id=akkadotnet;Password=akkadotnet;""
                            }
                        }
                    }";

        

        [TestCase()]
        public void SomeTest()
        {
            var testSystem = new TestKit(Config,"sql");
            SqlServerPersistence.Init(testSystem.Sys);

            using (var connection = new SqlConnection(@"Data Source = localhost\SQLEXPRESS; Database = AkkaPersistenceTest; User Id = akkadotnet; Password = akkadotnet;"))
            {
                var cleanSnapShotSql = "delete from SnapshotStore;";
                var cleanEventJournalSql = "delete from EventJournal;";
                connection.Open();

                using (var command = new SqlCommand(cleanSnapShotSql,connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(cleanEventJournalSql, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

            var persistor =
                testSystem.ActorOfAsTestActorRef<SqlTestActor>(Props.Create(() => new SqlTestActor("SomeTest")));
            var probe = testSystem.CreateTestProbe("testies");
            persistor.Tell(new SqlTestActor.Incrementor(10), probe);
            persistor.Tell(new SqlTestActor.RequestCurrentValue(), probe);

            var result = probe.ExpectMsg<SqlTestActor.IncrementorResult>();
            Assert.AreEqual(10,result.Value);

            testSystem.Shutdown(TimeSpan.FromSeconds(5), true);

            testSystem = new TestKit(Config, "sql");
             persistor = testSystem.ActorOfAsTestActorRef<SqlTestActor>(Props.Create(() => new SqlTestActor("SomeTest")));
             probe = testSystem.CreateTestProbe("testies2");
            persistor.Tell(new SqlTestActor.RequestCurrentValue(),probe);

            result = probe.ExpectMsg<SqlTestActor.IncrementorResult>();
            Assert.AreEqual(10, result.Value);

            persistor.Tell("snap",probe);
        //    probe.ExpectMsg<SaveSnapshotSuccess>();
            persistor.Tell(new SqlTestActor.Incrementor(10), probe);
            persistor.Tell(new SqlTestActor.RequestCurrentValue(), probe);

            result = probe.ExpectMsg<SqlTestActor.IncrementorResult>();
            Assert.AreEqual(20, result.Value);
            testSystem.Shutdown(TimeSpan.FromSeconds(5), true);

            testSystem = new TestKit(Config, "sql");
            persistor = testSystem.ActorOfAsTestActorRef<SqlTestActor>(Props.Create(() => new SqlTestActor("SomeTest")));
            probe = testSystem.CreateTestProbe("testies3");
            persistor.Tell(new SqlTestActor.RequestCurrentValue(), probe);

            result = probe.ExpectMsg<SqlTestActor.IncrementorResult>();
            Assert.AreEqual(20, result.Value);
        }

        internal class SqlTestActor : PersistentActor
        {
            private class IncrementorState
            {
                public IncrementorState(int value)
                {
                    Value = value;
                }

                public int Value { get;  }

                public override bool Equals(object obj)
                {
                    if (obj == null)
                        return false;

                    var state = obj as IncrementorState;
                    if (state == null)
                    {
                        return false;
                    }

                    return state.Value == Value;
                }

                public override int GetHashCode()
                {
                    return Value;
                }
            }

            private IncrementorState _state;

            public SqlTestActor(string persistenceId)
            {
                PersistenceId = persistenceId;
                _state = new IncrementorState(0);
            }


            protected override bool ReceiveRecover(object message)
            {
                var ev = message as Incrementor;
                if (ev != null)
                {
                    ProcessIncrementor(ev);
                    return true;
                }

                var snap = message as SnapshotOffer;
                if (snap != null)
                {
                    var state = (IncrementorState)snap.Snapshot;
                    if (!_state.Equals(state))
                    {
                        _state = state;
                    }
                    return true;
                }


                return false;
            }

            protected override bool ReceiveCommand(object message)
            {
                var incrementor = message as Incrementor;
                if (incrementor != null)
                {
                    Context.LogMessageDebug(message);
                    for (int i = 0; i < incrementor.Value; i++)
                    {
                        Persist(new Incrementor(1), ProcessIncrementor);
                    }
                    return true;
                }

                var request = message as RequestCurrentValue;
                if (request != null)
                {
                    Sender.Tell(new IncrementorResult(_state.Value));
                    return true;
                }


                if (message as string == "snap")
                {
                    SaveSnapshot(_state);
                    return true;
                }

                return false;
            }

            private void ProcessIncrementor(Incrementor obj)
            {
                _state = new IncrementorState(_state.Value + obj.Value);
            }

            public override string PersistenceId { get; }

            internal class IncrementorResult
            {
                public IncrementorResult(int value)
                {
                    Value = value;
                }

                public int Value { get;  }
            }

            internal class Incrementor
            {
                public int Value { get; }

                public Incrementor(int value)
                {
                    Value = value;
                }
            }

            internal class RequestCurrentValue
            {
            }
        }
    }
}