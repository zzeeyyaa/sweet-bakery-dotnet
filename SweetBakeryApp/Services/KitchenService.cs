using Microsoft.EntityFrameworkCore;
using SweetBakeryApp.Data;
using SweetBakeryApp.Models;

namespace SweetBakeryApp.Services;

public class KitchenService(BakeryDbContext context)
{
    // private readonly List<BakedProduct> _readyProducts = [];
    public (BakedProduct Product, List<Ingredient> UsedIngredients) CookOrder(OrderTicket ticket, CookingMethod method)
    {
        ticket.Status = OrderStatus.Cooking;

        Ingredient flour = new("Tepung Terigu", 250 * ticket.Quantity);
        Ingredient sugar = new("Gula Pasir", 150 * ticket.Quantity);

        List<Ingredient> ingredients = [flour, sugar];

        BakedProduct product = new(ticket, method);
        // _readyProducts.Add(product);
        context.BakedProducts.Add(product);

        ticket.Status = OrderStatus.Completed;

        context.SaveChanges();

        return (product, ingredients);
    }

    // public List<BakedProduct> GetReadyProducts() => _readyProducts;
    public List<BakedProduct> GetReadyProducts()
    {
        return [.. context.BakedProducts
        .Include(p=>p.OrderTicket)
        .ThenInclude(t=>t.Item)
        .OrderByDescending(p=>p.PreparedAt)];
    }
}