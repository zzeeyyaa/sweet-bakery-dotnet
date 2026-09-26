using SweetBakeryApp.Models;
using SweetBakeryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace SweetBakeryApp.Services;

public class CashierService(BakeryDbContext context)
{
    // private readonly List<OrderTicket> _activeOrders = [];

    public OrderTicket CreateOrder(MenuItem item, int quantity)
    {
        OrderTicket ticket = new(item, quantity);
        // _activeOrders.Add(ticket);
        // return ticket;
        context.OrderTickets.Add(ticket);
        context.SaveChanges();
        return ticket;
    }

    // public List<OrderTicket> GetActiveOrders() => _activeOrders;
    public List<OrderTicket> GetActiveOrders()
    {
        return [.. context.OrderTickets
        .Include(t => t.Item)
        .OrderByDescending(t => t.Id)];
    }
}