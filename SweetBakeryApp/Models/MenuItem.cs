namespace SweetBakeryApp.Models;

public class MenuItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;

    //foreign key ke category
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }

    // 1. Konstruktor kosong eksplisit untuk EF Core
    public MenuItem() { }

    // 2. Konstruktor utama untuk kode aplikasi
    public MenuItem(string name, decimal price, Guid categoryId, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Price = price;
        CategoryId = categoryId;
        IsActive = true;
    }
}