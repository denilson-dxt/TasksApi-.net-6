using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using Task.Data;
using Task.Dtos;
using Task.Models;
using Task.Services;
using Task.Authorization;

namespace Task.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly DataContext _context;
        private TaskService _taskService;
        public TaskController(DataContext context, TaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }
        
        [HttpPost]
        [Authorize()]
        public async Task<ActionResult<Models.Task>> Create(TaskDto request)
        {
            var user = (User)HttpContext.Items["User"];
            var task = await _taskService.CreateTask(request, user);
            return Ok(task);
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<Models.Task>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasks();
            return Ok(tasks);
        }
        
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetPostById(int id)
        {
            var task = await _taskService.GetTaskById(id);
            if (task == null)
                return BadRequest("Task not found");
            return Ok(task);
        }

        [Authorize]
        [HttpGet("user/")]
        public async Task<ActionResult<Models.Task>> GetTasksByUserId()
        {
            var user = (User)HttpContext.Items["User"];
            var tasks = await  _taskService.GetTasksByUserId(user.Id);
            return Ok(tasks);
        }

        [Authorize]
        [HttpPut()]
        public async Task<ActionResult<Models.Task>> UpdateTask(TaskDto request)
        {
            var user = (User)HttpContext.Items["User"];
            var task = await _taskService.UpdateTask(request, user);
            if (task == null)
                return BadRequest("Task not found");
            return Ok(task);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteTask(int id)
        {
            var user = (User)HttpContext.Items["User"];
            var deleted = await _taskService.DeleteTask(id, user);
            if (!deleted)
                return BadRequest("Task not found");
            return Ok(deleted);
        }
    }
}
