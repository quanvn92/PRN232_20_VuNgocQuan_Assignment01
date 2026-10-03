using _20_VuNgocQuan_Assignment01.DTOs;
using _20_VuNgocQuan_Assignment01.Models;
using _20_VuNgocQuan_Assignment01.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace _20_VuNgocQuan_Assignment01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ODataController
{
    private readonly ICategoryRepository _repo;

    public CategoriesController(ICategoryRepository repo)
    {
        _repo = repo;
    }

    /// <summary>Get all categories with optional keyword search</summary>
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll([FromQuery] string? keyword = null)
    {
        var categories = await _repo.SearchAsync(keyword);
        return Ok(categories.Select(MapToDto));
    }

    /// <summary>Get only active categories</summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetActive()
    {
        var categories = await _repo.GetActiveAsync();
        return Ok(categories.Select(MapToDto));
    }

    /// <summary>Get category by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(short id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(MapToDto(category));
    }

    /// <summary>Create category (Staff only)</summary>
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var category = new Category
        {
            CategoryName = dto.CategoryName,
            CategoryDesciption = dto.CategoryDesciption,
            ParentCategoryID = dto.ParentCategoryID,
            IsActive = dto.IsActive
        };

        await _repo.AddAsync(category);
        await _repo.SaveAsync();

        return CreatedAtAction(nameof(GetById), new { id = category.CategoryID }, MapToDto(category));
    }

    /// <summary>Update category (Staff only)</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(short id, [FromBody] UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var category = await _repo.GetByIdAsync(id);
        if (category == null) return NotFound();

        if (dto.CategoryName != null) category.CategoryName = dto.CategoryName;
        if (dto.CategoryDesciption != null) category.CategoryDesciption = dto.CategoryDesciption;
        if (dto.ParentCategoryID.HasValue) category.ParentCategoryID = dto.ParentCategoryID.Value;
        if (dto.IsActive.HasValue) category.IsActive = dto.IsActive.Value;

        await _repo.UpdateAsync(category);
        await _repo.SaveAsync();

        return NoContent();
    }

    /// <summary>Delete category. Fails if any news article references it.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(short id)
    {
        var category = await _repo.GetByIdAsync(id);
        if (category == null) return NotFound();

        var hasArticles = await _repo.HasNewsArticlesAsync(id);
        if (hasArticles)
            return BadRequest("Cannot delete category that is referenced by news articles.");

        await _repo.DeleteAsync(category);
        await _repo.SaveAsync();

        return NoContent();
    }

    private static CategoryDto MapToDto(Category c) => new()
    {
        CategoryID = c.CategoryID,
        CategoryName = c.CategoryName,
        CategoryDesciption = c.CategoryDesciption,
        ParentCategoryID = c.ParentCategoryID,
        ParentCategoryName = c.ParentCategory?.CategoryName,
        IsActive = c.IsActive
    };
}
