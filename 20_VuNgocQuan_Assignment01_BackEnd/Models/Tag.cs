using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20_VuNgocQuan_Assignment01.Models;

[Table("Tag")]
public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int TagID { get; set; }

    [MaxLength(50)]
    public string? TagName { get; set; }

    [MaxLength(400)]
    public string? Note { get; set; }

    // Navigation
    public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
}
