using WordSkills.Shared.Dtos.Identity;
using WordSkills.Client.Core.Controllers.Identity;

namespace WordSkills.Client.Core.Components.Pages.Identity.Profile;

public partial class ChangePasswordSection
{
    private bool isWaiting;
    private string? message;
    private readonly ChangePasswordRequestDto changeModel = new();


    [AutoInject] private IUserController userController = default!;


    [Parameter] public bool Loading { get; set; }

    private async Task ChangePassword()
    {
        if (isWaiting) return;

        isWaiting = true;
        message = null;

        try
        {
            await userController.ChangePassword(changeModel, CurrentCancellationToken);

            NavigationManager.NavigateTo("profile");
        }
        catch (KnownException e)
        {
            message = e.Message;
        }
        finally
        {
            isWaiting = false;
        }
    }
}
