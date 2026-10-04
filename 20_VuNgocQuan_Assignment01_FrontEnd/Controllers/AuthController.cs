using _20_VuNgocQuan_Assignment01_FrontEnd.DTOs;
using _20_VuNgocQuan_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _20_VuNgocQuan_Assignment01_FrontEnd.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _api;

    public AuthController(ApiService api) => _api = api;

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequestDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var result = await _api.LoginAsync(dto);
        if (result == null)
        {
            ViewBag.Error = "Invalid email or password.";
            return View(dto);
        }

        HttpContext.Session.SetString("AccountID", result.AccountID.ToString());
        HttpContext.Session.SetString("AccountName", result.AccountName ?? "");
        HttpContext.Session.SetString("AccountEmail", result.AccountEmail ?? "");
        HttpContext.Session.SetString("AccountRole", result.AccountRole?.ToString() ?? "-1");

        return result.AccountRole switch
        {
            0 => RedirectToAction("Index", "Admin"),
            1 => RedirectToAction("Index", "Staff"),
            _ => RedirectToAction("Index", "Home")
        };
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
