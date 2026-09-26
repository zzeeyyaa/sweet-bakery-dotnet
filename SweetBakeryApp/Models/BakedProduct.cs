namespace SweetBakeryApp.Models;

public class BakedProduct
{
    public Guid Id { get; init; } = Guid.NewGuid();

    // Foreign Key untuk SQL Server
    public Guid OrderTicketId { get; set; }
    public OrderTicket OrderTicket { get; set; } = null!;

    public CookingMethod Method { get; set; }
    public DateTime PreparedAt { get; set; } = DateTime.Now;

    public MenuItem? MenuItem => OrderTicket?.Item;
    public string Name => MenuItem?.Name ?? string.Empty;

    // 1. Konstruktor kosong eksplisit untuk EF Core (wajib ada)
    public BakedProduct() { }

    // 2. Konstruktor yang digunakan oleh kode aplikasi / KitchenService
    public BakedProduct(OrderTicket ticket, CookingMethod method)
    {
        OrderTicket = ticket;
        OrderTicketId = ticket.Id;
        Method = method;
        PreparedAt = DateTime.Now;
    }
}