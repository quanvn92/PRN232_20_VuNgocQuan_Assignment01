using _20_VuNgocQuan_Assignment01_FrontEnd.DTOs;
using _20_VuNgocQuan_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _20_VuNgocQuan_Assignment01_FrontEnd.Controllers;

/// <summary>Staff: Category management, News Article management, Profile, News History</summary>
public class StaffController : Controller
{
    private readonly ApiService _api;

    public StaffController(ApiService api) => _api = api;

    private bool IsStaff() => HttpContext.Session.GetString("AccountRole") == "1";

    private IActionResult RequireStaff()
    {
        if (!IsStaff()) return RedirectToAction("Login", "Auth");
        return null!;
    }

    private short CurrentAccountId()
        => short.TryParse(HttpContext.Session.GetString("AccountID"), out var id) ? id : (short)0;

    public IActionResult Index()
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        return View();
    }

    public async Task<IActionResult> Profile()
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var id = CurrentAccountId();
        var account = await _api.GetAccountAsync(id);
        if (account == null) return NotFound();
        ViewBag.AccountID = id;
        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> ProfileUpdate(UpdateSystemAccountDto dto)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var id = CurrentAccountId();
        var (ok, err) = await _api.UpdateAccountAsync(id, dto);
        if (!ok) TempData["Error"] = err;
        else
        {
            TempData["Success"] = "Profile updated successfully.";
            if (!string.IsNullOrEmpty(dto.AccountName))
                HttpContext.Session.SetString("AccountName", dto.AccountName);
        }
        return RedirectToAction("Profile");
    }

    public async Task<IActionResult> NewsHistory()
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var articles = await _api.GetNewsByCreatorAsync(CurrentAccountId());
        return View(articles);
    }

    public async Task<IActionResult> Categories(string? keyword)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var categories = await _api.GetCategoriesAsync(keyword);
        ViewBag.Keyword = keyword;
        return View(categories);
    }

    [HttpGet]
    public async Task<IActionResult> CategoryCreate()
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        ViewBag.ParentCategories = await _api.GetActiveCategoriesAsync();
        return PartialView("_CategoryFormPartial", new CreateCategoryDto());
    }

    [HttpPost]
    public async Task<IActionResult> CategoryCreate(CreateCategoryDto dto)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        if (!ModelState.IsValid)
        {
            ViewBag.ParentCategories = await _api.GetActiveCategoriesAsync();
            return PartialView("_CategoryFormPartial", dto);
        }
        var (ok, err) = await _api.CreateCategoryAsync(dto);
        if (!ok)
        {
            ModelState.AddModelError("", err ?? "Failed to create category.");
            ViewBag.ParentCategories = await _api.GetActiveCategoriesAsync();
            return PartialView("_CategoryFormPartial", dto);
        }
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> CategoryEdit(short id)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var cat = await _api.GetCategoryAsync(id);
        if (cat == null) return NotFound();
        var dto = new UpdateCategoryDto
        {
            CategoryName = cat.CategoryName,
            CategoryDesciption = cat.CategoryDesciption,
            ParentCategoryID = cat.ParentCategoryID,
            IsActive = cat.IsActive
        };
        ViewBag.CategoryID = id;
        ViewBag.ParentCategories = await _api.GetActiveCategoriesAsync();
        return PartialView("_CategoryEditPartial", dto);
    }

    [HttpPost]
    public async Task<IActionResult> CategoryEdit(short id, UpdateCategoryDto dto)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        if (!ModelState.IsValid)
        {
            ViewBag.CategoryID = id;
            ViewBag.ParentCategories = await _api.GetActiveCategoriesAsync();
            return PartialView("_CategoryEditPartial", dto);
        }
        var (ok, err) = await _api.UpdateCategoryAsync(id, dto);
        if (!ok)
        {
            ModelState.AddModelError("", err ?? "Failed to update category.");
            ViewBag.CategoryID = id;
            ViewBag.ParentCategories = await _api.GetActiveCategoriesAsync();
            return PartialView("_CategoryEditPartial", dto);
        }
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> CategoryDelete(short id)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var (ok, err) = await _api.DeleteCategoryAsync(id);
        return Json(new { success = ok, error = err });
    }

    public async Task<IActionResult> NewsArticles(string? keyword)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var articles = await _api.GetNewsArticlesAsync(keyword);
        ViewBag.Keyword = keyword;
        return View(articles);
    }

    [HttpGet]
    public async Task<IActionResult> NewsArticleCreate()
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        ViewBag.Categories = await _api.GetActiveCategoriesAsync();
        ViewBag.Tags = await _api.GetTagsAsync();
        return PartialView("_NewsArticleFormPartial", new CreateNewsArticleDto());
    }

    [HttpPost]
    public async Task<IActionResult> NewsArticleCreate(CreateNewsArticleDto dto, [FromForm] List<int>? TagIds)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        dto.CreatedByID = CurrentAccountId();
        dto.TagIds = TagIds ?? new List<int>();
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _api.GetActiveCategoriesAsync();
            ViewBag.Tags = await _api.GetTagsAsync();
            return PartialView("_NewsArticleFormPartial", dto);
        }
        var (ok, err) = await _api.CreateNewsArticleAsync(dto);
        if (!ok)
        {
            ModelState.AddModelError("", err ?? "Failed to create news article.");
            ViewBag.Categories = await _api.GetActiveCategoriesAsync();
            ViewBag.Tags = await _api.GetTagsAsync();
            return PartialView("_NewsArticleFormPartial", dto);
        }
        return Json(new { success = true });
    }

    [HttpGet]
    public async Task<IActionResult> NewsArticleEdit(string id)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var article = await _api.GetNewsArticleAsync(id);
        if (article == null) return NotFound();
        var dto = new UpdateNewsArticleDto
        {
            NewsTitle = article.NewsTitle,
            Headline = article.Headline,
            NewsContent = article.NewsContent,
            NewsSource = article.NewsSource,
            CategoryID = article.CategoryID,
            NewsStatus = article.NewsStatus,
            UpdatedByID = CurrentAccountId(),
            TagIds = article.Tags.Select(t => t.TagID).ToList()
        };
        ViewBag.ArticleID = id;
        ViewBag.Categories = await _api.GetActiveCategoriesAsync();
        ViewBag.Tags = await _api.GetTagsAsync();
        return PartialView("_NewsArticleEditPartial", dto);
    }

    [HttpPost]
    public async Task<IActionResult> NewsArticleEdit(string id, UpdateNewsArticleDto dto, [FromForm] List<int>? TagIds)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        dto.UpdatedByID = CurrentAccountId();
        dto.TagIds = TagIds ?? new List<int>();
        if (!ModelState.IsValid)
        {
            ViewBag.ArticleID = id;
            ViewBag.Categories = await _api.GetActiveCategoriesAsync();
            ViewBag.Tags = await _api.GetTagsAsync();
            return PartialView("_NewsArticleEditPartial", dto);
        }
        var (ok, err) = await _api.UpdateNewsArticleAsync(id, dto);
        if (!ok)
        {
            ModelState.AddModelError("", err ?? "Failed to update news article.");
            ViewBag.ArticleID = id;
            ViewBag.Categories = await _api.GetActiveCategoriesAsync();
            ViewBag.Tags = await _api.GetTagsAsync();
            return PartialView("_NewsArticleEditPartial", dto);
        }
        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> NewsArticleDelete(string id)
    {
        var guard = RequireStaff(); if (guard != null) return guard;
        var (ok, err) = await _api.DeleteNewsArticleAsync(id);
        return Json(new { success = ok, error = err });
    }
}
