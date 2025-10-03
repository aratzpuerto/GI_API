using GI_API.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GI_API.Services
{
    public class DbService
    {
        private readonly GIDbContext _context;

        public DbService(GIDbContext context) { _context = context; }

        public async Task<(bool TableExists, int? CurrentId)> ResetSeed(string tableName, int newSeedId)
        {
            // Check if the table exists
            var tableExists = await _context.Database.ExecuteSqlScalarAsync<int>($@"
                SELECT COUNT(1) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = '{tableName}';
            ");

            if (tableExists == 0)
                return (false, null);

            // Reset the identity seed
            await _context.Database.ExecuteSqlRawAsync($@"
                DBCC CHECKIDENT ('[{tableName}]', RESEED, {newSeedId});
            ");

            // Return the current identity value
            var currentId = await _context.Database.ExecuteSqlScalarAsync<int>($@"
                SELECT IDENT_CURRENT('{tableName}');
            ");

            return (true, currentId);
        }

        public async Task<(bool TableExists, int? CurrentId)> GetCurrentIdentity(string tableName)
        {
            // Check if the table exists
            var tableExists = await _context.Database.ExecuteSqlScalarAsync<int>($@"
                SELECT COUNT(1) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = '{tableName}';
            ");

            if (tableExists == 0)
                return (false, null);

            // Get current identity value
            var currentId = await _context.Database.ExecuteSqlScalarAsync<int>($@"
                SELECT IDENT_CURRENT('{tableName}');
            ");

            return (true, currentId);
        }
    }

    public static class DbContextExtensions
    {
        public static async Task<T> ExecuteSqlScalarAsync<T>(this DatabaseFacade database, string sql)
        {
            using var command = database.GetDbConnection().CreateCommand();
            command.CommandText = sql;
            if (command.Connection.State != System.Data.ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();
            return (T)Convert.ChangeType(result, typeof(T));
        }
    }
}
