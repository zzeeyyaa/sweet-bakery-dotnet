using SweetBakeryApp.Models;

namespace SweetBakeryApp.Models;

public class BakedProduct(OrderTicket ticket, CookingMethod method)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public OrderTicket OrderTicket { get; init; } = ticket;
    public CookingMethod Method { get; set; } = method;
    public DateTime PreparedAt { get; set; } = DateTime.Now;
    public MenuItem MenuItem => OrderTicket.Item;
    public string Name => MenuItem.Name;
}