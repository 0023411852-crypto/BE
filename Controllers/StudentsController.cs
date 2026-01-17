using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.Services;
using ProductApi.Domain.Entities;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentsController(StudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Get all students
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    /// <summary>
    /// Get student by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
        {
            return NotFound($"Student with ID {id} not found.");
        }

        return Ok(student);
    }

    /// <summary>
    /// Create a new student
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        var createdStudent = await _studentService.CreateStudentAsync(student);
        if (createdStudent == null)
        {
            return BadRequest("Invalid student data.");
        }

        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
    }

    /// <summary>
    /// Update an existing student
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<Student>> UpdateStudent(int id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest("ID mismatch.");
        }

        var updatedStudent = await _studentService.UpdateStudentAsync(student);
        if (updatedStudent == null)
        {
            return NotFound($"Student with ID {id} not found.");
        }

        return Ok(updatedStudent);
    }

    /// <summary>
    /// Delete a student
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        var deleted = await _studentService.DeleteStudentAsync(id);
        if (!deleted)
        {
            return NotFound($"Student with ID {id} not found.");
        }

        return NoContent();
    }
}