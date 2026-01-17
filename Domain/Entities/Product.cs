namespace ProductApi.Domain.Entities;

public class Class
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation property
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}