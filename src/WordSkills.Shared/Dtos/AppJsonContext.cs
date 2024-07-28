using WordSkills.Shared.Dtos.Todo;
using WordSkills.Shared.Dtos.Identity;
using WordSkills.Shared.Dtos.Glossary;

namespace WordSkills.Shared.Dtos;

/// <summary>
/// https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-source-generator/
/// </summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Dictionary<string, object>))]
[JsonSerializable(typeof(UserDto))]
[JsonSerializable(typeof(TodoItemDto))]
[JsonSerializable(typeof(PagedResult<TodoItemDto>))]
[JsonSerializable(typeof(List<TodoItemDto>))]
[JsonSerializable(typeof(WordDto))]
[JsonSerializable(typeof(PagedResult<WordDto>))]
[JsonSerializable(typeof(List<WordDto>))]
[JsonSerializable(typeof(IdentityRequestDto))]
[JsonSerializable(typeof(SignInRequestDto))]
[JsonSerializable(typeof(SignInResponseDto))]
[JsonSerializable(typeof(TokenResponseDto))]
[JsonSerializable(typeof(RefreshRequestDto))]
[JsonSerializable(typeof(SignUpRequestDto))]
[JsonSerializable(typeof(EditUserDto))]
[JsonSerializable(typeof(RestErrorInfo))]
[JsonSerializable(typeof(SendEmailTokenRequestDto))]
[JsonSerializable(typeof(SendPhoneTokenRequestDto))]
[JsonSerializable(typeof(ConfirmEmailRequestDto))]
[JsonSerializable(typeof(ChangeEmailRequestDto))]
[JsonSerializable(typeof(ConfirmPhoneRequestDto))]
[JsonSerializable(typeof(ChangePhoneNumberRequestDto))]
[JsonSerializable(typeof(SendResetPasswordTokenRequestDto))]
[JsonSerializable(typeof(ResetPasswordRequestDto))]
[JsonSerializable(typeof(TwoFactorAuthRequestDto))]
[JsonSerializable(typeof(TwoFactorAuthResponseDto))]
[JsonSerializable(typeof(ChangePasswordRequestDto))]
public partial class AppJsonContext : JsonSerializerContext
{
}
