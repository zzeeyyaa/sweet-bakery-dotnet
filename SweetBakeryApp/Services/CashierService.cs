using SweetBakeryApp.Models;

namespace SweetBakeryApp.Services;

public class CashierService
{
    private readonly List<OrderTicket> _activeOrders = [];

    public OrderTicket CreateOrder(MenuItem item, int quantity)
    {
        OrderTicket ticket = new(item, quantity);
        _activeOrders.Add(ticket);
        return ticket;
    }

    public List<OrderTicket> GetActiveOrders() => _activeOrders;
}