using _20_VuNgocQuan_Assignment01.DTOs;
using _20_VuNgocQuan_Assignment01.Models;
using _20_VuNgocQuan_Assignment01.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _20_VuNgocQuan_Assignment01.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISystemAccountRepository _accountRepo;
    private readonly IConfiguration _configuration;

    public AuthController(ISystemAccountRepository accountRepo, IConfiguration configuration)
    {
        _accountRepo = accountRepo;
        _configuration = configuration;
    }

    /// <summary>
    /// Login for Admin/Staff. Returns account info on success.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Email and password are required.");

        // Check admin account from appsettings
        var adminEmail = _configuration["AdminAccount:Email"];
        var adminPassword = _configuration["AdminAccount:Password"];

        if (request.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase)
            && request.Password == adminPassword)
        {
            return Ok(new LoginResponseDto
            {
                AccountID = 0,
                AccountName = "Administrator",
                AccountEmail = adminEmail,
                AccountRole = 0 // Admin role = 0
            });
        }

        // Check DB accounts
        var account = await _accountRepo.GetByEmailAsync(request.Email);
        if (account == null || account.AccountPassword != request.Password)
            return Unauthorized("Invalid email or password.");

        return Ok(new LoginResponseDto
        {
            AccountID = account.AccountID,
            AccountName = account.AccountName,
            AccountEmail = account.AccountEmail,
            AccountRole = account.AccountRole
        });
    }
}
