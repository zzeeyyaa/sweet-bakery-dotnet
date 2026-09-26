namespace SweetBakeryApp.Models;

public class MenuItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    // 1. Konstruktor kosong eksplisit untuk EF Core
    public MenuItem() { }

    // 2. Konstruktor utama untuk kode aplikasi
    public MenuItem(string name, decimal price, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Price = price;
    }
}