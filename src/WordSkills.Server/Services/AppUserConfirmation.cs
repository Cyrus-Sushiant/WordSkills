using WordSkills.Server.Models.Identity;

namespace WordSkills.Server.Services;

public class AppUserConfirmation : IUserConfirmation<User>
{
    public async Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
    {
        return user.EmailConfirmed || user.PhoneNumberConfirmed;
    }
}
