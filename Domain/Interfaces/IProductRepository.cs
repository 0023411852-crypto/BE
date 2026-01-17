using ProductApi.Domain.Entities;

namespace ProductApi.Domain.Interfaces;

public interface IClassRepository
{
    Task<IEnumerable<Class>> GetAllAsync();
    Task<Class?> GetByIdAsync(int id);
    Task<Class> CreateAsync(Class classEntity);
}