namespace _20_VuNgocQuan_Assignment01.DTOs;

// ── Auth ──────────────────────────────────────────────────────────────────
public class LoginRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public short AccountID { get; set; }
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
}

// ── SystemAccount ─────────────────────────────────────────────────────────
public class SystemAccountDto
{
    public short AccountID { get; set; }
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
}

public class CreateSystemAccountDto
{
    public short AccountID { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountEmail { get; set; } = string.Empty;
    public int AccountRole { get; set; }
    public string AccountPassword { get; set; } = string.Empty;
}

public class UpdateSystemAccountDto
{
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
    public string? AccountPassword { get; set; }
}

// ── Category ──────────────────────────────────────────────────────────────
public class CategoryDto
{
    public short CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDesciption { get; set; } = string.Empty;
    public short? ParentCategoryID { get; set; }
    public string? ParentCategoryName { get; set; }
    public bool? IsActive { get; set; }
}

public class CreateCategoryDto
{
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDesciption { get; set; } = string.Empty;
    public short? ParentCategoryID { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateCategoryDto
{
    public string? CategoryName { get; set; }
    public string? CategoryDesciption { get; set; }
    public short? ParentCategoryID { get; set; }
    public bool? IsActive { get; set; }
}

// ── Tag ───────────────────────────────────────────────────────────────────
public class TagDto
{
    public int TagID { get; set; }
    public string? TagName { get; set; }
    public string? Note { get; set; }
}

// ── NewsArticle ───────────────────────────────────────────────────────────
public class NewsArticleDto
{
    public string NewsArticleID { get; set; } = string.Empty;
    public string? NewsTitle { get; set; }
    public string Headline { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short? CategoryID { get; set; }
    public string? CategoryName { get; set; }
    public bool? NewsStatus { get; set; }
    public short? CreatedByID { get; set; }
    public string? CreatedByName { get; set; }
    public short? UpdatedByID { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<TagDto> Tags { get; set; } = new();
}

public class CreateNewsArticleDto
{
    public string NewsArticleID { get; set; } = string.Empty;
    public string? NewsTitle { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short? CategoryID { get; set; }
    public bool? NewsStatus { get; set; }
    public short? CreatedByID { get; set; }
    public List<int> TagIds { get; set; } = new();
}

public class UpdateNewsArticleDto
{
    public string? NewsTitle { get; set; }
    public string? Headline { get; set; }
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short? CategoryID { get; set; }
    public bool? NewsStatus { get; set; }
    public short? UpdatedByID { get; set; }
    public List<int>? TagIds { get; set; }
}

// ── Report ────────────────────────────────────────────────────────────────
public class ReportRequestDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
