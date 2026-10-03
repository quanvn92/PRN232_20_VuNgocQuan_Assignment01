using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20_VuNgocQuan_Assignment01.Models;

[Table("SystemAccount")]
public class SystemAccount
{
    [Key]
    public short AccountID { get; set; }

    [MaxLength(100)]
    public string? AccountName { get; set; }

    [MaxLength(70)]
    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }

    [MaxLength(70)]
    public string? AccountPassword { get; set; }

    // Navigation
    public ICollection<NewsArticle> CreatedArticles { get; set; } = new List<NewsArticle>();
}
