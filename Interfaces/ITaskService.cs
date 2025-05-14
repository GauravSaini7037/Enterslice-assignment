using TaskManagementApi.Models;

namespace TaskManagementApi.Interfaces
{
    public interface ITaskService
    {
        TaskModel SaveTask(TaskModel task);
        List<TaskModel> GetAllTasks();
        TaskModel GetTaskById(int id);
        void DeleteTask(int id);
    }
}
