namespace SweetBakeryApp.Models;

public class OrderTicket(MenuItem item, int quantity, Guid? id = null)
{
    public Guid Id { get; set; } = id ?? Guid.NewGuid();
    public MenuItem Item { get; set; } = item;
    public int Quantity { get; set; } = quantity;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    // hitung subtotal otomatis
    public decimal TotalPrice => Item.Price * Quantity;
}