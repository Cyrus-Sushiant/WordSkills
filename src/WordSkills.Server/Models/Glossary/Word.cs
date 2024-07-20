using WordSkills.Server.Models.Identity;

namespace WordSkills.Server.Models.Glossary;

public class Word
{
    public long Id { get; set; }

    [Required]
    public string? MainWord { get; set; }
    public string? Pronunciation { get; set; }
    public string? EnglishMeaning { get; set; }
    public string? PersianMeaning { get; set; }
    public string? Example { get; set; }
    public string? Description { get; set; }
    public WordType Type { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    public int UserId { get; set; }
}
