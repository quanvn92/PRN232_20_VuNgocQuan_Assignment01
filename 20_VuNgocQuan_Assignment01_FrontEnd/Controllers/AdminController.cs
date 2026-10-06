using _20_VuNgocQuan_Assignment01_FrontEnd.DTOs;
using _20_VuNgocQuan_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _20_VuNgocQuan_Assignment01_FrontEnd.Controllers;

/// <summary>Admin: Account management + Report</summary>
public class AdminController : Controller
{
    private readonly ApiService _api;

    public AdminController(ApiService api) => _api = api;

    private bool IsAdmin() => HttpContext.Session.GetString("AccountRole") == "0";

    private IActionResult RequireAdmin()
    {
        if (!IsAdmin()) return RedirectToAction("Login", "Auth");
        return null!;
    }

    public IActionResult Index()
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        return View();
    }

    public async Task<IActionResult> Accounts(string? keyword)
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        var accounts = await _api.GetAccountsAsync(keyword);
        ViewBag.Keyword = keyword;
        return View(accounts);
    }

    [HttpGet]
    public IActionResult AccountCreate()
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        return PartialView("_AccountFormPartial", new CreateSystemAccountDto());
    }

    [HttpPost]
    public async Task<IActionResult> AccountCreate(CreateSystemAccountDto dto)
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        if (!ModelState.IsValid) return PartialView("_AccountFormPartial", dto);

        var (ok, err) = await _api.CreateAccountAsync(dto);
        if (!ok)
        {
            ModelState.AddModelError("", err ?? "Failed to create account.");
            return PartialView("_AccountFormPartial", dto);
        }
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> AccountEdit(short id)
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        var account = await _api.GetAccountAsync(id);
        if (account == null) return NotFound();

        var dto = new UpdateSystemAccountDto
        {
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole
        };
        ViewBag.AccountID = id;
        return PartialView("_AccountEditPartial", dto);
    }

    [HttpPost]
    public async Task<IActionResult> AccountEdit(short id, UpdateSystemAccountDto dto)
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        if (!ModelState.IsValid)
        {
            ViewBag.AccountID = id;
            return PartialView("_AccountEditPartial", dto);
        }

        var (ok, err) = await _api.UpdateAccountAsync(id, dto);
        if (!ok)
        {
            ModelState.AddModelError("", err ?? "Failed to update account.");
            ViewBag.AccountID = id;
            return PartialView("_AccountEditPartial", dto);
        }
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> AccountDelete(short id)
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        var (ok, err) = await _api.DeleteAccountAsync(id);
        return Json(new { success = ok, error = err });
    }

    public IActionResult Report()
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Report(DateTime startDate, DateTime endDate)
    {
        var guard = RequireAdmin(); if (guard != null) return guard;
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;
        var articles = await _api.GetNewsReportAsync(startDate, endDate);
        return View(articles);
    }
}
