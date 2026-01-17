using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Services;
using ProductApi.Domain.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure EF Core with In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("SchoolManagementDatabase"));

// Register Repository and Service (Clean Architecture)
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<StudentService>();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "School Management API",
        Version = "v1",
        Description = "A RESTful API for managing Classes and Students using Clean Architecture"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management API v1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the root
});

// Comment out HTTPS redirection for development
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed some initial data for testing
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    if (!context.Classes.Any())
    {
        var classes = new[]
        {
            new ProductApi.Domain.Entities.Class { Name = "Computer Science", Description = "Learn programming and software development" },
            new ProductApi.Domain.Entities.Class { Name = "Mathematics", Description = "Advanced mathematical concepts" },
            new ProductApi.Domain.Entities.Class { Name = "Physics", Description = "Study of matter, energy, and their interactions" }
        };
        
        context.Classes.AddRange(classes);
        context.SaveChanges();
        
        // Add students after classes are saved to get their IDs
        context.Students.AddRange(
            new ProductApi.Domain.Entities.Student 
            { 
                FullName = "John Doe", 
                Email = "john.doe@email.com", 
                DateOfBirth = new DateTime(2000, 5, 15),
                ClassId = 1 
            },
            new ProductApi.Domain.Entities.Student 
            { 
                FullName = "Jane Smith", 
                Email = "jane.smith@email.com", 
                DateOfBirth = new DateTime(1999, 8, 22),
                ClassId = 1 
            },
            new ProductApi.Domain.Entities.Student 
            { 
                FullName = "Bob Johnson", 
                Email = "bob.johnson@email.com", 
                DateOfBirth = new DateTime(2001, 3, 10),
                ClassId = 2 
            }
        );
        context.SaveChanges();
    }
}

app.Run();