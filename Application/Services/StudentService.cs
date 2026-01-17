using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;

namespace ProductApi.Application.Services;

public class StudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _studentRepository.GetAllAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _studentRepository.GetByIdAsync(id);
    }

    public async Task<Student?> CreateStudentAsync(Student student)
    {
        if (student == null || string.IsNullOrWhiteSpace(student.FullName) || 
            string.IsNullOrWhiteSpace(student.Email) || student.ClassId <= 0)
            return null;

        return await _studentRepository.CreateAsync(student);
    }

    public async Task<Student?> UpdateStudentAsync(Student student)
    {
        if (student == null || student.Id <= 0 || string.IsNullOrWhiteSpace(student.FullName) || 
            string.IsNullOrWhiteSpace(student.Email) || student.ClassId <= 0)
            return null;

        return await _studentRepository.UpdateAsync(student);
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        if (id <= 0)
            return false;

        return await _studentRepository.DeleteAsync(id);
    }
}