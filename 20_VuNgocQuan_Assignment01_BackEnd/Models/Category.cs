using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20_VuNgocQuan_Assignment01.Models;

[Table("Category")]
public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public short CategoryID { get; set; }

    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string CategoryDesciption { get; set; } = string.Empty;

    public short? ParentCategoryID { get; set; }

    public bool? IsActive { get; set; }

    // Navigation
    [ForeignKey(nameof(ParentCategoryID))]
    public Category? ParentCategory { get; set; }

    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
