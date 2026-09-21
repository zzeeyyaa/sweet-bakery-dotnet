namespace SweetBakeryApp.Models;

public class MenuItem(string name, decimal price, Guid? id = null)
{
    public Guid Id { get; set; } = id ?? Guid.NewGuid();
    public string Name { get; set; } = name;
    public decimal Price { get; set; } = price;
}