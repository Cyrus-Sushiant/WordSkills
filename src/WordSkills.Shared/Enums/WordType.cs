namespace WordSkills.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<WordType>))]
public enum WordType : byte
{
    NotSet = 0,
    Noun,
    Verb,
    Adjective,
    Adverb
}
