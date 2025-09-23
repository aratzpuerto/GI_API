using Microsoft.Data.SqlClient;

namespace GI_API.Services
{
    public class DbService
    {
        static string _connectionName = "GI_Connection";


        public static async Task<int> ResetSeed(string tableName, int newSeedId, IConfiguration configuration)
        {
            string existsStr = @"
                    SELECT COUNT(1)
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_NAME = @tableName;";

            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString(_connectionName)))
            using (SqlCommand cmd = new SqlCommand(existsStr, con))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);

                await con.OpenAsync();

                int exists = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                if (exists == 0)
                    throw new Exception($"Table '{tableName}' does not exist in the database.");

                // Now safe to reset the identity
                string resetStr = $@"
                    DBCC CHECKIDENT ('[{tableName}]', RESEED, {newSeedId});
                    SELECT IDENT_CURRENT('[{tableName}]');";

                cmd.CommandText = resetStr;
                cmd.Parameters.Clear();

                object result = await cmd.ExecuteScalarAsync();

                if (result == null || result == DBNull.Value)
                    throw new Exception("ResetSeed failed: no identity value returned from database.");

                return Convert.ToInt32(result);
            }

        }
    }
}
