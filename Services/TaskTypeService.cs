using GI_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace GI_API.Services
{
    public class TaskTypeService
    {
//#if DEBUG
//        static string _connectionName = "TestConnection";
//#else
//        static string _connectionName = "GI_Connection";
//#endif

        public static List<TaskType> GetAll(IConfiguration configuration)
        {
            List<TaskType> taskTypes = new List<TaskType>();

            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("GI_Connection")))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TaskTypes", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TaskType taskType = new TaskType();
                        DataRow row = dt.Rows[i];

                        taskType.id = Convert.ToInt32(row["id"]);
                        taskType.name = Convert.ToString(row["name"]);
                        taskTypes.Add(taskType);
                    }
                }
            }

            return taskTypes;

        }


        public static TaskType GetById(int id, IConfiguration configuration)
        {

            TaskType taskType = new TaskType();

            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("GI_Connection")))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM TaskTypes WHERE id = @taskTypeID", con);
                da.SelectCommand.Parameters.AddWithValue("@taskTypeID", id);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    taskType.id = Convert.ToInt32(row["id"]);
                    taskType.name = Convert.ToString(row["name"]);

                }

            }

            return taskType;

        }

        public static async Task<int> SetTaskType(string name, IConfiguration configuration)
        {
            int newId;

            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("GI_Connection")))
            {
                SqlDataAdapter da = new SqlDataAdapter();
                da.InsertCommand = new SqlCommand("INSERT INTO TaskTypes (name) OUTPUT INSERTED.Id VALUES (@taskTypeName)", con);
                da.InsertCommand.Parameters.AddWithValue("@taskTypeName", name);

                await con.OpenAsync();
                object result = (int)await da.InsertCommand.ExecuteScalarAsync();

                // Extra safety check
                if (result == null || result == DBNull.Value)
                    throw new Exception("Insert failed: no Id returned from database.");

                newId = Convert.ToInt32(result);
            }

            return newId;

        }

        public static async Task<(int RowsAffected, string OldValue)> UpdateTaskType(int id, string name, IConfiguration configuration)
        {
            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("GI_Connection")))
            {
                await con.OpenAsync();

                using (SqlCommand updateCmd = new SqlCommand("UPDATE TaskTypes SET name = @taskTypeName OUTPUT DELETED.name as OldValue WHERE Id = @id", con))
                {
                    updateCmd.Parameters.AddWithValue("@taskTypeName", name);
                    updateCmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await updateCmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string oldValue = reader["OldValue"].ToString();
                            return (1, oldValue);
                        }
                    }

                }
            }

            return (0, null); // not found

        }

        public static async Task<(int RowsAffected, string DeletedValue)> DeleteTaskType(int id, IConfiguration configuration)
        {
            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("GI_Connection")))
            {
                await con.OpenAsync();

                using (SqlCommand deleteCmd = new SqlCommand("DELETE FROM TaskTypes OUTPUT DELETED.name AS DeletedValue WHERE Id = @id", con))
                {
                    deleteCmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = await deleteCmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string deletedValue = reader["DeletedValue"].ToString();
                            return (1, deletedValue);
                        }
                    }
                }
            }

            return (0, null); // not found
        }

    }


    //public TaskType? Get(int id) => TaskTypes.FirstOrDefault(p => p.id == id);

}
    

    //public class TaskTypeContext : DbContext
    //{
    //    public TaskTypeContext(DbContextOptions options) : base(options) { }

    //    public DbSet<TaskType> TaskTypes { get; set; }


    //}

