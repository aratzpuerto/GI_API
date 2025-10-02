using GI_API.Database;
using GI_API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace GI_API.Services
{
    public class TaskService
    {
        private readonly GIDbContext _context;
        public TaskService(GIDbContext context) { _context = context; }

        //static string _connectionName = "GI_Connection";

        public List<Models.Task> GetAll()
        {
            return _context.Tasks.ToList();
        }

        //public static List<Models.Task> GetAll(IConfiguration configuration)
        //{

        //    List<Models.Task> taskList = new List<Models.Task>();

        //    using (SqlConnection con = new SqlConnection(configuration.GetConnectionString(_connectionName)))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tasks", con);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                DataRow row = dt.Rows[i];
        //                Models.Task task = new Models.Task();

        //                task.id = Convert.ToInt32(row["id"]);
        //                task.name = Convert.ToString(row["name"]);

        //                task.description = row["description"] == DBNull.Value
        //                                    ? null
        //                                    : Convert.ToString(row["description"]);

        //                task.typeId = Convert.ToInt32(row["typeId"]);

        //                task.recurringEvery = row["recurringEvery"] == DBNull.Value
        //                                        ? (int?)null
        //                                        : Convert.ToInt32(row["recurringEvery"]);

        //                task.showOrder = Convert.ToInt32(row["showOrder"]);
        //                task.show = Convert.ToBoolean(row["show"]);
        //                task.completed = Convert.ToBoolean(row["completed"]);
        //                task.completionDate = row["completionDate"] == DBNull.Value
        //                                        ? (DateTime?)null
        //                                        : Convert.ToDateTime(row["completionDate"]);

        //                task.active = Convert.ToBoolean(row["active"]);

        //                taskList.Add(task);
        //            }
        //        }
        //    }

        //    return taskList;

        //}

        public Models.Task? GetById(int id)
        {
            return _context.Tasks.Find(id);
        }

        //public static Models.Task GetById(int id, IConfiguration configuration)
        //{

        //    Models.Task task = new Models.Task();

        //    using (SqlConnection con = new SqlConnection(configuration.GetConnectionString(_connectionName)))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Tasks WHERE id = @taskID", con);
        //        da.SelectCommand.Parameters.AddWithValue("@taskID", id);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        if (dt.Rows.Count > 0)
        //        {
        //            DataRow row = dt.Rows[0];

        //            task.id = Convert.ToInt32(row["id"]);
        //            task.name = Convert.ToString(row["name"]);

        //            task.description = row["description"] == DBNull.Value
        //                                ? null
        //                                : Convert.ToString(row["description"]);

        //            task.typeId = Convert.ToInt32(row["typeId"]);

        //            task.recurringEvery = row["recurringEvery"] == DBNull.Value
        //                                    ? (int?)null
        //                                    : Convert.ToInt32(row["recurringEvery"]);

        //            task.showOrder = Convert.ToInt32(row["showOrder"]);
        //            task.show = Convert.ToBoolean(row["show"]);
        //            task.completed = Convert.ToBoolean(row["completed"]);
        //            task.completionDate = row["completionDate"] == DBNull.Value
        //                                    ? (DateTime?)null
        //                                    : Convert.ToDateTime(row["completionDate"]);

        //            task.active = Convert.ToBoolean(row["active"]);
        //        }

        //    }

        //    return task;

        //}


        public async Task<int> SetTask(string name, string? description, int typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active)
        {
            var task = new Models.Task
            {
                Name = name,
                TypeId = typeId,
            };

            if (description != null) task.Description = description;
            if (recurringEvery != null) task.RecurringEvery = recurringEvery;
            if (showOrder != null) task.ShowOrder = showOrder.Value;
            if (show != null) task.Show = show.Value;
            if (completed != null) task.Completed = completed.Value;
            if (completionDate != null) task.CompletionDate = completionDate;
            if (active != null) task.Active = active.Value;
            
            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();
            return task.Id;
        }

        //public static async Task<int> SetTask(string name, string? description, int typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active,  IConfiguration configuration)
        //{
        //    int newId;

        //    string insertStr = "INSERT INTO Tasks (name,";
        //    if (description != null) insertStr += " description,";
        //    insertStr += " typeId";
        //    if (recurringEvery != null) insertStr += " ,recurringEvery";
        //    if (showOrder != null) insertStr += " ,showOrder";
        //    if (show != null) insertStr += " ,show";
        //    if (completed != null) insertStr += " ,completed";
        //    if (completionDate != null) insertStr += " ,completionDate";
        //    if (active != null) insertStr += " ,active";
        //    insertStr += ") OUTPUT INSERTED.Id VALUES (@name,";
        //    if (description != null) insertStr += " @description,";
        //    insertStr += " @typeId";
        //    if (recurringEvery != null) insertStr += " ,@recurringEvery";
        //    if (showOrder != null) insertStr += " ,@showOrder";
        //    if (show != null) insertStr += " ,@show";
        //    if (completed != null) insertStr += " ,@completed";
        //    if (completionDate != null) insertStr += " ,@completionDate";
        //    if (active != null) insertStr += " ,@active";
        //    insertStr += ")";

        //    using (SqlConnection con = new SqlConnection(configuration.GetConnectionString(_connectionName)))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.InsertCommand = new SqlCommand(insertStr, con);
        //        da.InsertCommand.Parameters.AddWithValue("@name", name);
        //        if (description != null) da.InsertCommand.Parameters.AddWithValue("@description", description);
        //        da.InsertCommand.Parameters.AddWithValue("@typeId", typeId);
        //        if (recurringEvery != null) da.InsertCommand.Parameters.AddWithValue("@recurringEvery", recurringEvery);
        //        if (showOrder != null) da.InsertCommand.Parameters.AddWithValue("@showOrder", showOrder);
        //        if (show != null) da.InsertCommand.Parameters.AddWithValue("@show", show);
        //        if (completed != null) da.InsertCommand.Parameters.AddWithValue("@completed", completed);
        //        if (completionDate != null) da.InsertCommand.Parameters.AddWithValue("@completionDate", completionDate);
        //        if (active != null) da.InsertCommand.Parameters.AddWithValue("@active", active);

        //        await con.OpenAsync();
        //        object result = (int)await da.InsertCommand.ExecuteScalarAsync();

        //        // Extra safety check
        //        if (result == null || result == DBNull.Value)
        //            throw new Exception("Insert failed: no Id returned from database.");

        //        newId = Convert.ToInt32(result);
        //    }

        //    return newId;

        //}

        public async Task<int> UpdateTask(int id, string? name, string? description, int? typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active)
        {
            var entity = await _context.Tasks.FindAsync(id);
            if (entity == null)
                return 0;
            if (!string.IsNullOrEmpty(name)) entity.Name = name;
            if (typeId != null) entity.TypeId = (int)typeId;
            if (description != null) entity.Description = description;
            if (recurringEvery != null) entity.RecurringEvery = recurringEvery;
            if (showOrder != null) entity.ShowOrder = showOrder.Value;
            if (show != null) entity.Show = show.Value;
            if (completed != null) entity.Completed = completed.Value;
            if (completionDate != null) entity.CompletionDate = completionDate;
            if (active != null) entity.Active = active.Value;
            await _context.SaveChangesAsync();
            return 1;
        }


        //public static async Task<int> UpdateTask(int id, string? name, string? description, int? typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active, IConfiguration configuration)
        //{
        //    string updatetStr = "UPDATE Tasks SET ";

        //    List<string> setParams = new List<string>();
        //    if (name != null) setParams.Add("name = @name");
        //    if (description != null) setParams.Add("description = @description");
        //    if (typeId != null) setParams.Add("typeId = @typeId");
        //    if (recurringEvery != null) setParams.Add("recurringEvery = @recurringEvery");
        //    if (showOrder != null) setParams.Add("showOrder = @showOrder");
        //    if (show != null) setParams.Add("show = @show");
        //    if (completed != null) setParams.Add("completed = @completed");
        //    if (completionDate != null) setParams.Add("completionDate = @completionDate");
        //    if (active != null) setParams.Add("active = @active");

        //    updatetStr += string.Join(", ", setParams);
        //    updatetStr += " WHERE id = @id";

        //    using (SqlConnection con = new SqlConnection(configuration.GetConnectionString(_connectionName)))
        //    {
        //        await con.OpenAsync();

        //        using (SqlCommand updateCmd = new SqlCommand(updatetStr, con))
        //        {



        //            if (name != null) updateCmd.Parameters.AddWithValue("@name", name);
        //            if (description != null) updateCmd.Parameters.AddWithValue("@description", description);
        //            if (typeId != null) updateCmd.Parameters.AddWithValue("@typeId", typeId);
        //            if (recurringEvery != null) updateCmd.Parameters.AddWithValue("@recurringEvery", recurringEvery);
        //            if (showOrder != null) updateCmd.Parameters.AddWithValue("@showOrder", showOrder);
        //            if (show != null) updateCmd.Parameters.AddWithValue("@show", show);
        //            if (completed != null) updateCmd.Parameters.AddWithValue("@completed", completed);
        //            if (completionDate != null) updateCmd.Parameters.AddWithValue("@completionDate", completionDate);
        //            if (active != null) updateCmd.Parameters.AddWithValue("@active", active);

        //            updateCmd.Parameters.AddWithValue("@id", id);

        //            int rowsAffected = await updateCmd.ExecuteNonQueryAsync();

        //            return rowsAffected;

        //        }
        //    }

        //    return 0; 

        //}


        public async Task<int> DeleteTask(int id)
        {
            var entity = await _context.Tasks.FindAsync(id);
            if (entity == null)
                return 0;
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync();
            return 1;
        }

        //public static async Task<int> DeleteTask(int id, IConfiguration configuration)
        //{
        //    string deleteStr = @"
        //            DELETE FROM Tasks 
        //            WHERE Id = @id;";

        //    using (SqlConnection con = new SqlConnection(configuration.GetConnectionString(_connectionName)))
        //    {
        //        await con.OpenAsync();

        //        using (SqlCommand deleteCmd = new SqlCommand(deleteStr, con))
        //        {
        //            deleteCmd.Parameters.AddWithValue("@id", id);

        //            int rowsAffected = await deleteCmd.ExecuteNonQueryAsync();

        //            return rowsAffected;

        //        }
        //    }

        //    return 0;
        //}

    }
}
