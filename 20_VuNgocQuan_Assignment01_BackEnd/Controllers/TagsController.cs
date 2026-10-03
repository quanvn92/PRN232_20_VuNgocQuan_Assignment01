using _20_VuNgocQuan_Assignment01.DTOs;
using _20_VuNgocQuan_Assignment01.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace _20_VuNgocQuan_Assignment01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ODataController
{
    private readonly ITagRepository _repo;

    public TagsController(ITagRepository repo)
    {
        _repo = repo;
    }

    /// <summary>Get all tags</summary>
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetAll()
    {
        var tags = await _repo.GetAllAsync();
        return Ok(tags.Select(t => new TagDto
        {
            TagID = t.TagID,
            TagName = t.TagName,
            Note = t.Note
        }));
    }

    /// <summary>Get tag by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetById(int id)
    {
        var tag = await _repo.GetByIdAsync(id);
        if (tag == null) return NotFound();
        return Ok(new TagDto { TagID = tag.TagID, TagName = tag.TagName, Note = tag.Note });
    }
}
