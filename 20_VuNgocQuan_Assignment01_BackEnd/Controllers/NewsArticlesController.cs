using _20_VuNgocQuan_Assignment01.DTOs;
using _20_VuNgocQuan_Assignment01.Models;
using _20_VuNgocQuan_Assignment01.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace _20_VuNgocQuan_Assignment01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsArticlesController : ODataController
{
    private readonly INewsArticleRepository _repo;

    public NewsArticlesController(INewsArticleRepository repo)
    {
        _repo = repo;
    }

    /// <summary>Get all news articles with optional keyword search (public can view active only)</summary>
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<NewsArticleDto>>> GetAll([FromQuery] string? keyword = null)
    {
        var articles = await _repo.SearchAsync(keyword);
        return Ok(articles.Select(MapToDto));
    }

    /// <summary>Get only active news articles (public access)</summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<NewsArticleDto>>> GetActive()
    {
        var articles = await _repo.GetActiveNewsAsync();
        return Ok(articles.Select(MapToDto));
    }

    /// <summary>Get news articles by creator account ID (Staff history)</summary>
    [HttpGet("by-creator/{accountId}")]
    public async Task<ActionResult<IEnumerable<NewsArticleDto>>> GetByCreator(short accountId)
    {
        var articles = await _repo.GetByCreatedByAsync(accountId);
        return Ok(articles.Select(MapToDto));
    }

    /// <summary>Get news articles by date range for Admin report</summary>
    [HttpGet("report")]
    public async Task<ActionResult<IEnumerable<NewsArticleDto>>> GetReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (startDate > endDate)
            return BadRequest("StartDate must be before or equal to EndDate.");

        var articles = await _repo.GetByDateRangeAsync(startDate, endDate.AddDays(1).AddSeconds(-1));
        return Ok(articles.Select(MapToDto));
    }

    /// <summary>Get news article by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<NewsArticleDto>> GetById(string id)
    {
        var article = await _repo.GetByIdAsync(id);
        if (article == null) return NotFound();
        return Ok(MapToDto(article));
    }

    /// <summary>Create news article (Staff only)</summary>
    [HttpPost]
    public async Task<ActionResult<NewsArticleDto>> Create([FromBody] CreateNewsArticleDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (string.IsNullOrWhiteSpace(dto.NewsArticleID))
            return BadRequest("NewsArticleID is required.");

        // Check duplicate ID
        var existing = await _repo.GetByIdAsync(dto.NewsArticleID);
        if (existing != null)
            return Conflict($"News article with ID '{dto.NewsArticleID}' already exists.");

        var article = new NewsArticle
        {
            NewsArticleID = dto.NewsArticleID,
            NewsTitle = dto.NewsTitle,
            Headline = dto.Headline,
            NewsContent = dto.NewsContent,
            NewsSource = dto.NewsSource,
            CategoryID = dto.CategoryID,
            NewsStatus = dto.NewsStatus,
            CreatedByID = dto.CreatedByID,
            UpdatedByID = dto.CreatedByID,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        await _repo.AddAsync(article);
        await _repo.SaveAsync();

        // Add tags
        if (dto.TagIds.Any())
        {
            await _repo.UpdateTagsAsync(article.NewsArticleID, dto.TagIds);
            await _repo.SaveAsync();
        }

        var created = await _repo.GetByIdAsync(article.NewsArticleID);
        return CreatedAtAction(nameof(GetById), new { id = article.NewsArticleID }, MapToDto(created!));
    }

    /// <summary>Update news article (Staff only)</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateNewsArticleDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var article = await _repo.GetByIdAsync(id);
        if (article == null) return NotFound();

        if (dto.NewsTitle != null) article.NewsTitle = dto.NewsTitle;
        if (dto.Headline != null) article.Headline = dto.Headline;
        if (dto.NewsContent != null) article.NewsContent = dto.NewsContent;
        if (dto.NewsSource != null) article.NewsSource = dto.NewsSource;
        if (dto.CategoryID.HasValue) article.CategoryID = dto.CategoryID.Value;
        if (dto.NewsStatus.HasValue) article.NewsStatus = dto.NewsStatus.Value;
        if (dto.UpdatedByID.HasValue) article.UpdatedByID = dto.UpdatedByID.Value;
        article.ModifiedDate = DateTime.Now;

        await _repo.UpdateAsync(article);

        // Update tags if provided
        if (dto.TagIds != null)
        {
            await _repo.UpdateTagsAsync(id, dto.TagIds);
        }

        await _repo.SaveAsync();

        return NoContent();
    }

    /// <summary>Delete news article (Staff only)</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var article = await _repo.GetByIdAsync(id);
        if (article == null) return NotFound();

        await _repo.DeleteAsync(article);
        await _repo.SaveAsync();

        return NoContent();
    }

    private static NewsArticleDto MapToDto(NewsArticle n) => new()
    {
        NewsArticleID = n.NewsArticleID,
        NewsTitle = n.NewsTitle,
        Headline = n.Headline,
        CreatedDate = n.CreatedDate,
        NewsContent = n.NewsContent,
        NewsSource = n.NewsSource,
        CategoryID = n.CategoryID,
        CategoryName = n.Category?.CategoryName,
        NewsStatus = n.NewsStatus,
        CreatedByID = n.CreatedByID,
        CreatedByName = n.CreatedBy?.AccountName,
        UpdatedByID = n.UpdatedByID,
        ModifiedDate = n.ModifiedDate,
        Tags = n.NewsTags.Select(nt => new TagDto
        {
            TagID = nt.Tag!.TagID,
            TagName = nt.Tag.TagName,
            Note = nt.Tag.Note
        }).ToList()
    };
}
