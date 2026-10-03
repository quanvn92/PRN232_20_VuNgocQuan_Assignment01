using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20_VuNgocQuan_Assignment01.Models;

[Table("NewsArticle")]
public class NewsArticle
{
    [Key]
    [MaxLength(20)]
    public string NewsArticleID { get; set; } = string.Empty;

    [MaxLength(400)]
    public string? NewsTitle { get; set; }

    [Required]
    [MaxLength(150)]
    public string Headline { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; }

    [MaxLength(4000)]
    public string? NewsContent { get; set; }

    [MaxLength(400)]
    public string? NewsSource { get; set; }

    public short? CategoryID { get; set; }

    public bool? NewsStatus { get; set; }

    public short? CreatedByID { get; set; }

    public short? UpdatedByID { get; set; }

    public DateTime? ModifiedDate { get; set; }

    // Navigation
    [ForeignKey(nameof(CategoryID))]
    public Category? Category { get; set; }

    [ForeignKey(nameof(CreatedByID))]
    public SystemAccount? CreatedBy { get; set; }

    public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
}
