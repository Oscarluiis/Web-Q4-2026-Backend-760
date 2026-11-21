using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoQ4Backend760.DTOs;
using ProyectoQ4Backend760.Services;

namespace ProyectoQ4Backend760.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        _taskService = taskService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var task = await _taskService.Create(dto, userId);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetMyTasks()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var tasks = await _taskService.GetByUser(userId);
            
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}