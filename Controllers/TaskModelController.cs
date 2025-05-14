using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;

namespace TaskManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllTasks()
        {
            var tasks = _taskService.GetAllTasks();
            return Ok(tasks);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetTaskById(int id)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        [Route("[action]")]

        public IActionResult SaveTask([FromBody] TaskModel task)
        {
            if (task == null)
                return BadRequest("Task is null");

            var savedTask = _taskService.SaveTask(task);
            return Ok(savedTask);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult DeleteTask(int id)
        {
             _taskService.DeleteTask(id);
            return Ok();
        }
    }
}
