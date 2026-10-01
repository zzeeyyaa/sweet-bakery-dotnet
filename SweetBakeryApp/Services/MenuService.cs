using Microsoft.EntityFrameworkCore;
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

    // CATEGORY OPERATIONS
    public List<Category> GetAllCategories()
    {
        return [.. context.Categories.OrderBy(c => c.Name)];
    }

    public void AddCategory(string name, string description = "")
    {
        context.Categories.Add(new Category(name, description));
        context.SaveChanges();
    }
    public void UpdateCategory(Guid id, string name, string description)
    {
        var category = context.Categories.Find(id);
        if (category != null)
        {
            category.Name = name;
            category.Description = description;
            context.SaveChanges();
        }
    }
    public void DeleteCategory(Guid id)
    {
        var category = context.Categories.Find(id);
        if (category != null && !category.MenuItems.Any(m => m.IsActive))
        {
            context.Categories.Remove(category);
            context.SaveChanges();
        }
    }

    // MENU ITEM OPERATIONS
    public List<MenuItem> GetAllMenu()
    {
        return [.. context.MenuItems
        .Include(m=>m.Category)
        .Where(m=>m.IsActive)
        .OrderBy(m=>m.Category!.Name)
        .ThenBy(m=>m.Name)];
    }
    public void AddMenuItem(string name, decimal price, Guid categoryId)
    {
        // _menuCatalog.Add(new(name, price));
        MenuItem item = new(name, price, categoryId);
        context.MenuItems.Add(item);
        context.SaveChanges();
    }

    public void UpdateMenuItem(Guid id, string name, decimal price, Guid categoryId)
    {
        var item = context.MenuItems.Find(id);
        if (item != null)
        {
            item.Name = name;
            item.Price = price;
            item.CategoryId = categoryId;
            context.SaveChanges();
        }
    }
    public void DeleteMenuItem(Guid id)
    {
        var item = context.MenuItems.Find(id);
        if (item != null)
        {
            context.MenuItems.Remove(item);
            context.SaveChanges();
        }
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
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category("Roti & Pastry", "Aneka roti segar dan pastry"),
                new Category("Cake", "Kue tart dan slice manis"),
                new Category("Minuman", "Kopi dan minuman dingin")
            );
            context.SaveChanges();

        }
        if (!context.MenuItems.Any())
        {
            var pastryCat = context.Categories.FirstOrDefault(c => c.Name == "Roti & Pastry");
            var cakeCat = context.Categories.FirstOrDefault(c => c.Name == "Cake");
            var beverageCat = context.Categories.FirstOrDefault(c => c.Name == "Minuman");
            var initialMenus = new List<MenuItem>();
            if (pastryCat != null)
            {
                initialMenus.Add(new MenuItem("Croissant Mentega", 18000, pastryCat.Id));
                initialMenus.Add(new MenuItem("Roti Cokelat Keju", 12000, pastryCat.Id));
            }
            if (cakeCat != null)
            {
                initialMenus.Add(new MenuItem("Cheesecake Slice", 35000, cakeCat.Id));
            }
            if (beverageCat != null)
            {
                initialMenus.Add(new MenuItem("Iced Caffe Latte", 22000, beverageCat.Id));
            }

            if (initialMenus.Count != 0)
            {
                context.MenuItems.AddRange(initialMenus);
                context.SaveChanges();
            }
        }
    }
}