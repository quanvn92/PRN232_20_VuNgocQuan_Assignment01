using System.Net.Http.Json;
using _20_VuNgocQuan_Assignment01_FrontEnd.DTOs;

namespace _20_VuNgocQuan_Assignment01_FrontEnd.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("API");
    }

    // ── Auth ─────────────────────────────────────────────────────────────
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/auth/login", dto);
        if (res.IsSuccessStatusCode)
            return await res.Content.ReadFromJsonAsync<LoginResponseDto>();
        return null;
    }

    // ── SystemAccounts ───────────────────────────────────────────────────
    public async Task<List<SystemAccountDto>> GetAccountsAsync(string? keyword = null)
    {
        var url = string.IsNullOrWhiteSpace(keyword)
            ? "api/systemaccounts"
            : $"api/systemaccounts?keyword={Uri.EscapeDataString(keyword)}";
        return await _http.GetFromJsonAsync<List<SystemAccountDto>>(url) ?? new();
    }

    public async Task<SystemAccountDto?> GetAccountAsync(short id)
        => await _http.GetFromJsonAsync<SystemAccountDto>($"api/systemaccounts/{id}");

    public async Task<(bool ok, string? error)> CreateAccountAsync(CreateSystemAccountDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/systemaccounts", dto);
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    public async Task<(bool ok, string? error)> UpdateAccountAsync(short id, UpdateSystemAccountDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/systemaccounts/{id}", dto);
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    public async Task<(bool ok, string? error)> DeleteAccountAsync(short id)
    {
        var res = await _http.DeleteAsync($"api/systemaccounts/{id}");
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    // ── Categories ───────────────────────────────────────────────────────
    public async Task<List<CategoryDto>> GetCategoriesAsync(string? keyword = null)
    {
        var url = string.IsNullOrWhiteSpace(keyword)
            ? "api/categories"
            : $"api/categories?keyword={Uri.EscapeDataString(keyword)}";
        return await _http.GetFromJsonAsync<List<CategoryDto>>(url) ?? new();
    }

    public async Task<List<CategoryDto>> GetActiveCategoriesAsync()
        => await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories/active") ?? new();

    public async Task<CategoryDto?> GetCategoryAsync(short id)
        => await _http.GetFromJsonAsync<CategoryDto>($"api/categories/{id}");

    public async Task<(bool ok, string? error)> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/categories", dto);
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    public async Task<(bool ok, string? error)> UpdateCategoryAsync(short id, UpdateCategoryDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/categories/{id}", dto);
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    public async Task<(bool ok, string? error)> DeleteCategoryAsync(short id)
    {
        var res = await _http.DeleteAsync($"api/categories/{id}");
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    // ── Tags ─────────────────────────────────────────────────────────────
    public async Task<List<TagDto>> GetTagsAsync()
        => await _http.GetFromJsonAsync<List<TagDto>>("api/tags") ?? new();

    // ── NewsArticles ─────────────────────────────────────────────────────
    public async Task<List<NewsArticleDto>> GetNewsArticlesAsync(string? keyword = null)
    {
        var url = string.IsNullOrWhiteSpace(keyword)
            ? "api/newsarticles"
            : $"api/newsarticles?keyword={Uri.EscapeDataString(keyword)}";
        return await _http.GetFromJsonAsync<List<NewsArticleDto>>(url) ?? new();
    }

    public async Task<List<NewsArticleDto>> GetActiveNewsAsync()
        => await _http.GetFromJsonAsync<List<NewsArticleDto>>("api/newsarticles/active") ?? new();

    public async Task<List<NewsArticleDto>> GetNewsByCreatorAsync(short accountId)
        => await _http.GetFromJsonAsync<List<NewsArticleDto>>($"api/newsarticles/by-creator/{accountId}") ?? new();

    public async Task<List<NewsArticleDto>> GetNewsReportAsync(DateTime startDate, DateTime endDate)
    {
        var url = $"api/newsarticles/report?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
        return await _http.GetFromJsonAsync<List<NewsArticleDto>>(url) ?? new();
    }

    public async Task<NewsArticleDto?> GetNewsArticleAsync(string id)
        => await _http.GetFromJsonAsync<NewsArticleDto>($"api/newsarticles/{Uri.EscapeDataString(id)}");

    public async Task<(bool ok, string? error)> CreateNewsArticleAsync(CreateNewsArticleDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/newsarticles", dto);
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    public async Task<(bool ok, string? error)> UpdateNewsArticleAsync(string id, UpdateNewsArticleDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/newsarticles/{Uri.EscapeDataString(id)}", dto);
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }

    public async Task<(bool ok, string? error)> DeleteNewsArticleAsync(string id)
    {
        var res = await _http.DeleteAsync($"api/newsarticles/{Uri.EscapeDataString(id)}");
        if (res.IsSuccessStatusCode) return (true, null);
        var err = await res.Content.ReadAsStringAsync();
        return (false, err);
    }
}
