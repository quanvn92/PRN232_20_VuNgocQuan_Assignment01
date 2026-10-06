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

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/auth/login", dto);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadFromJsonAsync<LoginResponseDto>();
        }
        catch { }
        return null;
    }

    public async Task<List<SystemAccountDto>> GetAccountsAsync(string? keyword = null)
    {
        try
        {
            var url = string.IsNullOrWhiteSpace(keyword)
                ? "api/systemaccounts"
                : $"api/systemaccounts?keyword={Uri.EscapeDataString(keyword)}";
            return await _http.GetFromJsonAsync<List<SystemAccountDto>>(url) ?? new();
        }
        catch { return new(); }
    }

    public async Task<SystemAccountDto?> GetAccountAsync(short id)
    {
        try
        {
            return await _http.GetFromJsonAsync<SystemAccountDto>($"api/systemaccounts/{id}");
        }
        catch { return null; }
    }

    public async Task<(bool ok, string? error)> CreateAccountAsync(CreateSystemAccountDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/systemaccounts", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool ok, string? error)> UpdateAccountAsync(short id, UpdateSystemAccountDto dto)
    {
        try
        {
            var res = await _http.PutAsJsonAsync($"api/systemaccounts/{id}", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool ok, string? error)> DeleteAccountAsync(short id)
    {
        try
        {
            var res = await _http.DeleteAsync($"api/systemaccounts/{id}");
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(string? keyword = null)
    {
        try
        {
            var url = string.IsNullOrWhiteSpace(keyword)
                ? "api/categories"
                : $"api/categories?keyword={Uri.EscapeDataString(keyword)}";
            return await _http.GetFromJsonAsync<List<CategoryDto>>(url) ?? new();
        }
        catch { return new(); }
    }

    public async Task<List<CategoryDto>> GetActiveCategoriesAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<CategoryDto>>("api/categories/active") ?? new();
        }
        catch { return new(); }
    }

    public async Task<CategoryDto?> GetCategoryAsync(short id)
    {
        try
        {
            return await _http.GetFromJsonAsync<CategoryDto>($"api/categories/{id}");
        }
        catch { return null; }
    }

    public async Task<(bool ok, string? error)> CreateCategoryAsync(CreateCategoryDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/categories", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool ok, string? error)> UpdateCategoryAsync(short id, UpdateCategoryDto dto)
    {
        try
        {
            var res = await _http.PutAsJsonAsync($"api/categories/{id}", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool ok, string? error)> DeleteCategoryAsync(short id)
    {
        try
        {
            var res = await _http.DeleteAsync($"api/categories/{id}");
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<List<TagDto>> GetTagsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<TagDto>>("api/tags") ?? new();
        }
        catch { return new(); }
    }

    public async Task<List<NewsArticleDto>> GetNewsArticlesAsync(string? keyword = null)
    {
        try
        {
            var url = string.IsNullOrWhiteSpace(keyword)
                ? "api/newsarticles"
                : $"api/newsarticles?keyword={Uri.EscapeDataString(keyword)}";
            return await _http.GetFromJsonAsync<List<NewsArticleDto>>(url) ?? new();
        }
        catch { return new(); }
    }

    public async Task<List<NewsArticleDto>> GetActiveNewsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<NewsArticleDto>>("api/newsarticles/active") ?? new();
        }
        catch { return new(); }
    }

    public async Task<List<NewsArticleDto>> GetNewsByCreatorAsync(short accountId)
    {
        try
        {
            return await _http.GetFromJsonAsync<List<NewsArticleDto>>($"api/newsarticles/by-creator/{accountId}") ?? new();
        }
        catch { return new(); }
    }

    public async Task<List<NewsArticleDto>> GetNewsReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var url = $"api/newsarticles/report?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            return await _http.GetFromJsonAsync<List<NewsArticleDto>>(url) ?? new();
        }
        catch { return new(); }
    }

    public async Task<NewsArticleDto?> GetNewsArticleAsync(string id)
    {
        try
        {
            return await _http.GetFromJsonAsync<NewsArticleDto>($"api/newsarticles/{Uri.EscapeDataString(id)}");
        }
        catch { return null; }
    }

    public async Task<(bool ok, string? error)> CreateNewsArticleAsync(CreateNewsArticleDto dto)
    {
        try
        {
            var res = await _http.PostAsJsonAsync("api/newsarticles", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool ok, string? error)> UpdateNewsArticleAsync(string id, UpdateNewsArticleDto dto)
    {
        try
        {
            var res = await _http.PutAsJsonAsync($"api/newsarticles/{Uri.EscapeDataString(id)}", dto);
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }

    public async Task<(bool ok, string? error)> DeleteNewsArticleAsync(string id)
    {
        try
        {
            var res = await _http.DeleteAsync($"api/newsarticles/{Uri.EscapeDataString(id)}");
            if (res.IsSuccessStatusCode) return (true, null);
            var err = await res.Content.ReadAsStringAsync();
            return (false, err);
        }
        catch (Exception ex) { return (false, ex.Message); }
    }
}
