namespace WordSkills.Shared.Dtos.Glossary;
public class WordDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = nameof(AppStrings.RequiredAttribute_ValidationError))]
    public string? MainWord { get; set; }
    public string? Pronunciation { get; set; }
    public string? EnglishMeaning { get; set; }
    public string? PersianMeaning { get; set; }
    public string? Example { get; set; }
    public string? Description { get; set; }
    public WordType Type { get; set; }

    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }

    [JsonIgnore]
    public bool IsInEditMode { get; set; }
}
