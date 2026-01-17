using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly ClassService _classService;

    public ClassesController(ClassService classService)
    {
        _classService = classService;
    }

    /// <summary>
    /// Get all classes
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
    {
        var classes = await _classService.GetAllClassesAsync();
        return Ok(classes);
    }

    /// <summary>
    /// Get class by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Class>> GetClass(int id)
    {
        var classEntity = await _classService.GetClassByIdAsync(id);
        if (classEntity == null)
        {
            return NotFound($"Class with ID {id} not found.");
        }

        return Ok(classEntity);
    }

    /// <summary>
    /// Create a new class
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Class>> CreateClass(Class classEntity)
    {
        var createdClass = await _classService.CreateClassAsync(classEntity);
        if (createdClass == null)
        {
            return BadRequest("Invalid class data.");
        }

        return CreatedAtAction(nameof(GetClass), new { id = createdClass.Id }, createdClass);
    }
}