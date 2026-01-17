using ProductApi.Domain.Entities;

namespace ProductApi.Domain.Interfaces;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<Student> CreateAsync(Student student);
    Task<Student?> UpdateAsync(Student student);
    Task<bool> DeleteAsync(int id);
}