using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly string _connectionString;
        public TaskService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnection");
        }
        public TaskModel SaveTask(TaskModel task)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("sp_SaveOrUpdateTask", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", task.Id);
                    cmd.Parameters.AddWithValue("@TaskTitle", task.TaskTitle ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TaskDescription", task.TaskDescription ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TaskDueDate", task.TaskDueDate);
                    cmd.Parameters.AddWithValue("@TaskStatus", task.TaskStatus ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TaskRemarks", task.TaskRemarks ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedByName", task.CreatedByName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedById", task.CreatedById);
                    cmd.Parameters.AddWithValue("@LastUpdatedByName", task.LastUpdatedByName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastUpdatedById", task.LastUpdatedById);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return task;
            }
        }
        public void DeleteTask(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new SqlCommand("DELETE FROM Tasks WHERE Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<TaskModel> GetAllTasks()
        {
            var tasks = new List<TaskModel>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new SqlCommand("SELECT * FROM Tasks", conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskModel model = new TaskModel();
                            model.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                            model.TaskTitle = reader.GetString(reader.GetOrdinal("TaskTitle"));
                            model.TaskDescription = reader.IsDBNull(reader.GetOrdinal("TaskDescription")) ? null : reader.GetString(reader.GetOrdinal("TaskDescription"));
                            model.TaskDueDate = reader.GetDateTime(reader.GetOrdinal("TaskDueDate"));
                            model.TaskStatus = reader.GetString(reader.GetOrdinal("TaskStatus"));
                            model.TaskRemarks = reader.IsDBNull(reader.GetOrdinal("TaskRemarks")) ? null : reader.GetString(reader.GetOrdinal("TaskRemarks"));
                            model.CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn"));
                            model.LastUpdatedOn = reader.GetDateTime(reader.GetOrdinal("LastUpdatedOn"));
                            model.CreatedByName = reader.GetString(reader.GetOrdinal("CreatedByName"));
                            model.CreatedById = reader.GetInt32(reader.GetOrdinal("CreatedById"));
                            model.LastUpdatedByName = reader.GetString(reader.GetOrdinal("LastUpdatedByName"));
                            model.LastUpdatedById = reader.GetInt32(reader.GetOrdinal("LastUpdatedById"));
                            tasks.Add(model);
                        }
                    }
                }
            }
            return tasks;
        }

        public TaskModel GetTaskById(int id)
        {
            TaskModel tasks = new TaskModel();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new SqlCommand("SELECT * FROM Tasks WHERE Id = @Id", conn))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TaskModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                TaskTitle = reader.GetString(reader.GetOrdinal("TaskTitle")),
                                TaskDescription = reader.IsDBNull(reader.GetOrdinal("TaskDescription")) ? null : reader.GetString(reader.GetOrdinal("TaskDescription")),
                                TaskDueDate = reader.GetDateTime(reader.GetOrdinal("TaskDueDate")),
                                TaskStatus = reader.GetString(reader.GetOrdinal("TaskStatus")),
                                TaskRemarks = reader.IsDBNull(reader.GetOrdinal("TaskRemarks")) ? null : reader.GetString(reader.GetOrdinal("TaskRemarks")),
                                CreatedOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn")),
                                LastUpdatedOn = reader.GetDateTime(reader.GetOrdinal("LastUpdatedOn")),
                                CreatedByName = reader.GetString(reader.GetOrdinal("CreatedByName")),
                                CreatedById = reader.GetInt32(reader.GetOrdinal("CreatedById")),
                                LastUpdatedByName = reader.GetString(reader.GetOrdinal("LastUpdatedByName")),
                                LastUpdatedById = reader.GetInt32(reader.GetOrdinal("LastUpdatedById"))
                            };
                        }
                        return null; // if Task not found
                    }
                }
            }
        }
    }
}
