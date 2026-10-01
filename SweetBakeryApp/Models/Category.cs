namespace SweetBakeryApp.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<MenuItem> MenuItems { get; set; } = [];

    public Category() { }

    public Category(string name, string description = "", Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Description = description;
    }
}