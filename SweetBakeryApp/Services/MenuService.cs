using SweetBakeryApp.Models;
namespace SweetBakeryApp.Services;

public class MenuService
{
    private readonly List<MenuItem> _menuCatalog = [
        new("Red Velvet Tart", 45000m),
        new("Fudge Brownies", 35000m),
        new("Matcha Roll Cake", 40000m)
    ];
    public List<MenuItem> GetAllMenu() => _menuCatalog;
    public void AddNewMenu(string name, decimal price)
    {
        _menuCatalog.Add(new(name, price));
    }

    //pencarian
    public MenuItem? FindMenu(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return null;
        }
        return _menuCatalog.First(m =>
        // Cocokkan dengan nama kue (mengabaikan huruf besar/kecil & boleh sebagian kata)
        m.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
        ||
        // ATAU cocokkan dengan 8 karakter kode Guid jika user mengetik kode ID
        (query.Length >= 4 && m.Id.ToString()[..8].StartsWith(query, StringComparison.OrdinalIgnoreCase))
        );

    }
}