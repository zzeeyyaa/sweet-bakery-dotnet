using SweetBakeryApp.Data;
using SweetBakeryApp.Models;
namespace SweetBakeryApp.Services;

public class MenuService(BakeryDbContext context)
{
    // private readonly List<MenuItem> _menuCatalog = [
    //     new("Red Velvet Tart", 45000m),
    //     new("Fudge Brownies", 35000m),
    //     new("Matcha Roll Cake", 40000m)
    // ];
    // public List<MenuItem> GetAllMenu() => _menuCatalog;
    public List<MenuItem> GetAllMenu()
    {
        return [.. context.MenuItems];
    }
    public void AddNewMenu(string name, decimal price)
    {
        // _menuCatalog.Add(new(name, price));
        MenuItem item = new(name, price);
        context.MenuItems.Add(item);
        context.SaveChanges();
    }

    //pencarian
    public MenuItem? FindMenu(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return null;
        }
        return context.MenuItems.AsEnumerable().FirstOrDefault(m =>
        // Cocokkan dengan nama kue (mengabaikan huruf besar/kecil & boleh sebagian kata)
        m.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
        ||
        // ATAU cocokkan dengan 8 karakter kode Guid jika user mengetik kode ID
        (query.Length >= 4 && m.Id.ToString()[..8].StartsWith(query, StringComparison.OrdinalIgnoreCase))
        );

    }

    public void SeedInitialMenu()
    {
        if (!context.MenuItems.Any())
        {
            context.MenuItems.AddRange(
                new("Red Velvet Tart", 45000m),
                new("Fudge Brownies", 35000m),
                new("Matcha Roll Cake", 40000m)
            );
            context.SaveChanges();
        }
    }
}