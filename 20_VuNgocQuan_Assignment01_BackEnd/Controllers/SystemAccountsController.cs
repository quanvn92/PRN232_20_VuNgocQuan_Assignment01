using _20_VuNgocQuan_Assignment01.DTOs;
using _20_VuNgocQuan_Assignment01.Models;
using _20_VuNgocQuan_Assignment01.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace _20_VuNgocQuan_Assignment01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemAccountsController : ODataController
{
    private readonly ISystemAccountRepository _repo;

    public SystemAccountsController(ISystemAccountRepository repo)
    {
        _repo = repo;
    }

    /// <summary>Get all accounts (Admin only)</summary>
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<SystemAccountDto>>> GetAll([FromQuery] string? keyword = null)
    {
        var accounts = await _repo.SearchAsync(keyword);
        return Ok(accounts.Select(MapToDto));
    }

    /// <summary>Get account by ID</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemAccountDto>> GetById(short id)
    {
        var account = await _repo.GetByIdAsync(id);
        if (account == null) return NotFound();
        return Ok(MapToDto(account));
    }

    /// <summary>Create account (Admin only)</summary>
    [HttpPost]
    public async Task<ActionResult<SystemAccountDto>> Create([FromBody] CreateSystemAccountDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Check if ID already exists
        var existing = await _repo.GetByIdAsync(dto.AccountID);
        if (existing != null)
            return Conflict($"Account with ID {dto.AccountID} already exists.");

        // Check if email already exists
        var byEmail = await _repo.GetByEmailAsync(dto.AccountEmail);
        if (byEmail != null)
            return Conflict("An account with this email already exists.");

        var account = new SystemAccount
        {
            AccountID = dto.AccountID,
            AccountName = dto.AccountName,
            AccountEmail = dto.AccountEmail,
            AccountRole = dto.AccountRole,
            AccountPassword = dto.AccountPassword
        };

        await _repo.AddAsync(account);
        await _repo.SaveAsync();

        return CreatedAtAction(nameof(GetById), new { id = account.AccountID }, MapToDto(account));
    }

    /// <summary>Update account (Admin only)</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(short id, [FromBody] UpdateSystemAccountDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var account = await _repo.GetByIdAsync(id);
        if (account == null) return NotFound();

        if (dto.AccountName != null) account.AccountName = dto.AccountName;
        if (dto.AccountEmail != null) account.AccountEmail = dto.AccountEmail;
        if (dto.AccountRole.HasValue) account.AccountRole = dto.AccountRole.Value;
        if (dto.AccountPassword != null) account.AccountPassword = dto.AccountPassword;

        await _repo.UpdateAsync(account);
        await _repo.SaveAsync();

        return NoContent();
    }

    /// <summary>Delete account (Admin only). Fails if account has created any articles.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(short id)
    {
        var account = await _repo.GetByIdAsync(id);
        if (account == null) return NotFound();

        var hasArticles = await _repo.HasCreatedArticlesAsync(id);
        if (hasArticles)
            return BadRequest("Cannot delete account that has created news articles.");

        await _repo.DeleteAsync(account);
        await _repo.SaveAsync();

        return NoContent();
    }

    private static SystemAccountDto MapToDto(SystemAccount a) => new()
    {
        AccountID = a.AccountID,
        AccountName = a.AccountName,
        AccountEmail = a.AccountEmail,
        AccountRole = a.AccountRole
    };
}
