using Microsoft.AspNetCore.Mvc;
using Task;
using Microsoft.EntityFrameworkCore;
using Task.Data;
using Task.Dtos;
using Task.Models;

namespace Services;

public class TaskService
{
    private DataContext _context;
    public TaskService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Task.Models.Task>> GetAllTasks()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return tasks;
    }

    public async Task<Task.Models.Task> GetTaskById(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async Task<Task.Models.Task> CreateTask(TaskDto request, User user)
    {
        var task = new Task.Models.Task();
        task.Name = request.Name;
        task.User = user;
        task.Description = request.Description;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return task;
    }

    public async Task<List<Task.Models.Task>> GetTasksByUserId(int userId)
    {
        var tasks = await _context.Tasks.Where(t => t.User.Id == userId).ToListAsync();
        return tasks;
    }

    public async Task<Task.Models.Task?> UpdateTask(TaskDto request, User user)
    {
        if (!await _context.Tasks.Where(t => t.Id == request.Id && t.User.Id == user.Id).AnyAsync())
        {
            return null;
        }
        var task = await _context.Tasks.Where(t=>t.Id == request.Id && t.User.Id == user.Id).FirstAsync();
        task.Name = request.Name;
        task.Description = request.Description;
        task.Day = request.Day;

        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTask(int id, User user)
    {
        if (!await _context.Tasks.Where(t => t.Id == id && t.User.Id == user.Id).AnyAsync())
        {
            return false;
        }
        var task = await _context.Tasks.Where(t=>t.Id == id && t.User.Id == user.Id).FirstAsync();
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;

    }

}