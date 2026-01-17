using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Infrastructure.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students.Include(s => s.Class).ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Class)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Student> CreateAsync(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student?> UpdateAsync(Student student)
    {
        var existingStudent = await _context.Students.FindAsync(student.Id);
        if (existingStudent == null)
            return null;

        existingStudent.FullName = student.FullName;
        existingStudent.Email = student.Email;
        existingStudent.DateOfBirth = student.DateOfBirth;
        existingStudent.ClassId = student.ClassId;

        await _context.SaveChangesAsync();
        return existingStudent;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return false;

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
}