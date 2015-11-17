using System.Data.SqlClient;

namespace Entities.Model
{
    class SqlPersistenceHelpers
    {
        public static void ClearDatabase()
        {
            using (
                var connection =
                    new SqlConnection(
                        @"Data Source = localhost\SQLEXPRESS; Database = AkkaPersistenceTest; User Id = akkadotnet; Password = akkadotnet;")
                )
            {
                var cleanSnapShotSql = "delete from SnapshotStore;";
                var cleanEventJournalSql = "delete from EventJournal;";
                connection.Open();

                using (var command = new SqlCommand(cleanSnapShotSql, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqlCommand(cleanEventJournalSql, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}