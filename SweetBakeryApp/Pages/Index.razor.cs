using Microsoft.AspNetCore.Components;
using SweetBakeryApp.Models;
using SweetBakeryApp.Services;

namespace SweetBakeryApp.Pages;

public partial class Index : ComponentBase
{
    [Inject]
    public MenuService MenuService { get; set; } = null!;

    protected List<MenuItem> menuItems = [];
    protected List<Category> categories = [];

    // State Navigasi Tab di Admin
    protected string activeTab = "menu"; // "menu" atau "category"

    // Form Menu
    protected Guid? editingMenuId = null;
    protected string menuName = string.Empty;
    protected decimal menuPrice;
    protected Guid selectedCategoryId;

    // Form Kategori
    protected Guid? editingCategoryId = null;
    protected string categoryName = string.Empty;
    protected string categoryDescription = string.Empty;

    protected string feedbackMessage = string.Empty;

    protected override void OnInitialized()
    {
        RefreshData();
    }

    protected void RefreshData()
    {
        categories = MenuService.GetAllCategories();
        menuItems = MenuService.GetAllMenu();

        if (categories.Any() && selectedCategoryId == Guid.Empty)
        {
            selectedCategoryId = categories.First().Id;
        }
    }

    // --- Action Menu ---
    protected void SaveMenu()
    {
        if (string.IsNullOrWhiteSpace(menuName) || menuPrice <= 0 || selectedCategoryId == Guid.Empty)
        {
            feedbackMessage = "Nama menu, harga, dan kategori wajib diisi!";
            return;
        }

        if (editingMenuId.HasValue)
        {
            MenuService.UpdateMenuItem(editingMenuId.Value, menuName, menuPrice, selectedCategoryId);
            feedbackMessage = "Menu berhasil diperbarui!";
        }
        else
        {
            MenuService.AddMenuItem(menuName, menuPrice, selectedCategoryId);
            feedbackMessage = "Menu baru berhasil ditambahkan!";
        }

        ResetMenuForm();
        RefreshData();
    }

    protected void EditMenu(MenuItem item)
    {
        editingMenuId = item.Id;
        menuName = item.Name;
        menuPrice = item.Price;
        selectedCategoryId = item.CategoryId;
    }

    protected void DeleteMenu(Guid id)
    {
        MenuService.DeleteMenuItem(id);
        feedbackMessage = "Menu berhasil dinonaktifkan.";
        RefreshData();
    }

    protected void ResetMenuForm()
    {
        editingMenuId = null;
        menuName = string.Empty;
        menuPrice = 0;
        if (categories.Any()) selectedCategoryId = categories.First().Id;
    }

    // --- Action Kategori ---
    protected void SaveCategory()
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            feedbackMessage = "Nama kategori tidak boleh kosong!";
            return;
        }

        if (editingCategoryId.HasValue)
        {
            MenuService.UpdateCategory(editingCategoryId.Value, categoryName, categoryDescription);
            feedbackMessage = "Kategori berhasil diperbarui!";
        }
        else
        {
            MenuService.AddCategory(categoryName, categoryDescription);
            feedbackMessage = "Kategori baru berhasil ditambahkan!";
        }

        ResetCategoryForm();
        RefreshData();
    }

    protected void EditCategory(Category cat)
    {
        editingCategoryId = cat.Id;
        categoryName = cat.Name;
        categoryDescription = cat.Description;
    }

    protected void DeleteCategory(Guid id)
    {
        MenuService.DeleteCategory(id);
        feedbackMessage = "Kategori berhasil dihapus.";
        RefreshData();
    }

    protected void ResetCategoryForm()
    {
        editingCategoryId = null;
        categoryName = string.Empty;
        categoryDescription = string.Empty;
    }
}