namespace SweetBakeryApp.Models;

public class OrderTicket
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Foreign Key ke MenuItem
    public Guid MenuItemId { get; set; }
    public MenuItem Item { get; set; } = null!;

    public int Quantity { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public decimal TotalPrice => Item != null ? Item.Price * Quantity : 0;

    // Konstruktor kosong untuk EF Core
    public OrderTicket() { }

    // Konstruktor aplikasi
    public OrderTicket(MenuItem item, int quantity, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Item = item;
        MenuItemId = item.Id;
        Quantity = quantity;
    }
}