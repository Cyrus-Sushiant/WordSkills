using WordSkills.Client.Core.Controllers.Identity;

namespace WordSkills.Client.Core.Components.Pages.Identity.Profile;

public partial class DeleteAccountConfirmModal
{
    [AutoInject] IUserController userController = default!;

    [Parameter] public bool IsOpen { get; set; }

    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    private async Task CloseModal()
    {
        IsOpen = false;

        await IsOpenChanged.InvokeAsync(false);
    }

    private async Task DeleteAccount()
    {
        await userController.Delete(CurrentCancellationToken);

        await AuthenticationManager.SignOut();

        await CloseModal();
    }
}
