using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private List<TaskModel> tasks = new List<TaskModel>();
    private readonly DataRepository dataRepository = new DataRepository();

    public TaskController()
    {
        // При запуске контроллера загружаем задачи из файла по умолчанию (JSON)
        tasks = dataRepository.LoadTasks("tasks.json");
    }

    [HttpPost]
    public IActionResult AddTask([FromBody] TaskModel task)
    {
        tasks.Add(task);
        dataRepository.SaveTasks(tasks, "tasks.json");
        return Ok();
    }

    [HttpGet("search")]
    public IActionResult SearchTasks([FromQuery] string keywords)
    {
        var result = tasks
            .Where(t => t.Tags.Any(tag => keywords.Split(' ').Contains(tag)))
            .OrderBy(t => t.DueDate)
            .Take(5)
            .ToList();

        return Ok(result);
    }

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        return Ok(tasks);
    }

    [HttpPost("upload")]
    public IActionResult UploadTasks([FromForm] string fileType, [FromForm] Microsoft.AspNetCore.Http.IFormFile file)
    {
        if (fileType.ToLower() == "json")
        {
            tasks = dataRepository.LoadTasksFromJson(file);
            return Ok();
        }
        else if (fileType.ToLower() == "xml")
        {
            tasks = dataRepository.LoadTasksFromXml(file);
            return Ok();
        }
        else
        {
            return BadRequest("Unsupported file type");
        }
    }
}