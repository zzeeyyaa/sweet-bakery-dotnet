using Microsoft.AspNetCore.Mvc;
using SweetBakeryApp.Services;

namespace SweetBakeryApp.Controllers;

public class MenuController(MenuService menuService) : Controller
{
    public IActionResult Index()
    {
        var menus = menuService.GetAllMenu();
        return View(menus);
    }
}