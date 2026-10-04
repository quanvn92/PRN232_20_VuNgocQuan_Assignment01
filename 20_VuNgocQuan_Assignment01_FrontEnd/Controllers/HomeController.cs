using _20_VuNgocQuan_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _20_VuNgocQuan_Assignment01_FrontEnd.Controllers;

public class HomeController : Controller
{
    private readonly ApiService _api;

    public HomeController(ApiService api) => _api = api;

    // Public: show all active news articles
    public async Task<IActionResult> Index()
    {
        var news = await _api.GetActiveNewsAsync();
        return View(news);
    }

    // Public: view single news article detail
    public async Task<IActionResult> Detail(string id)
    {
        var article = await _api.GetNewsArticleAsync(id);
        if (article == null || article.NewsStatus != true)
            return NotFound();
        return View(article);
    }
}
