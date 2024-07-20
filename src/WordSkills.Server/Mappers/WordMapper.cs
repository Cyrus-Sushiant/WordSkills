using Riok.Mapperly.Abstractions;
using WordSkills.Server.Models.Glossary;
using WordSkills.Shared.Dtos.Glossary;

namespace WordSkills.Server.Mappers;

/// <summary>
/// More info at Server/Mappers/README.md
/// </summary>
[Mapper(UseDeepCloning = true)]
public static partial class WordMapper
{
    public static partial IQueryable<WordDto> Project(this IQueryable<Word> query);
    public static partial WordDto Map(this Word source);
    public static partial Word Map(this WordDto source);
    public static partial void Patch(this WordDto source, Word destination);
}
