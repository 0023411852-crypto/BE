using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;

namespace ProductApi.Application.Services;

public class ClassService
{
    private readonly IClassRepository _classRepository;

    public ClassService(IClassRepository classRepository)
    {
        _classRepository = classRepository;
    }

    public async Task<IEnumerable<Class>> GetAllClassesAsync()
    {
        return await _classRepository.GetAllAsync();
    }

    public async Task<Class?> GetClassByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _classRepository.GetByIdAsync(id);
    }

    public async Task<Class?> CreateClassAsync(Class classEntity)
    {
        if (classEntity == null || string.IsNullOrWhiteSpace(classEntity.Name))
            return null;

        return await _classRepository.CreateAsync(classEntity);
    }
}