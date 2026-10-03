using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20_VuNgocQuan_Assignment01.Models;

[Table("NewsTag")]
public class NewsTag
{
    [MaxLength(20)]
    public string NewsArticleID { get; set; } = string.Empty;

    public int TagID { get; set; }

    // Navigation
    [ForeignKey(nameof(NewsArticleID))]
    public NewsArticle? NewsArticle { get; set; }

    [ForeignKey(nameof(TagID))]
    public Tag? Tag { get; set; }
}
