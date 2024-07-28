using WordSkills.Client.Core.Controllers.Glossary;
using WordSkills.Shared.Dtos.Glossary;

namespace WordSkills.Client.Core.Components.Pages.Glossary;

[Authorize]
public partial class WordPage
{
    [AutoInject] Keyboard keyboard = default!;
    [AutoInject] IWordController wordController = default!;

    private bool isAdding;
    private bool isLoading;
    private string? searchText;
    private string? selectedSort;
    private string? selectedFilter;
    private WordDto wordModel = new();
    private BitSnackBar snackBar = default!;
    private ConfirmMessageBox confirmMessageBox = default!;
    private IList<WordDto> allWords = default!;
    private IList<WordDto> viewWords = default!;
    private List<BitDropdownItem<string>> sortItems = [];
    private BitSearchBox searchBox = default!;

    protected override async Task OnInitAsync()
    {
        _ = keyboard.Add(ButilKeyCodes.KeyF, () => _ = searchBox.FocusAsync(), ButilModifiers.Ctrl);

        selectedFilter = nameof(AppStrings.All);
        selectedSort = nameof(AppStrings.Date);

        sortItems =
        [
            new BitDropdownItem<string> { Text = Localizer[nameof(AppStrings.Alphabetical)], Value = nameof(AppStrings.Alphabetical) },
            new BitDropdownItem<string> { Text = Localizer[nameof(AppStrings.Date)], Value = nameof(AppStrings.Date) }
        ];

        await LoadTodoItems();

        await base.OnInitAsync();
    }

    private async Task LoadTodoItems()
    {
        isLoading = true;

        try
        {
            allWords = await wordController.Get(CurrentCancellationToken);

            FilterViewTodoItems();
        }
        finally
        {
            isLoading = false;
        }
    }

    private void FilterViewTodoItems()
    {
        viewWords = allWords
            .Where(t => WordIsVisible(t))
            .OrderByIf(selectedSort == nameof(AppStrings.Alphabetical), t => t.MainWord!)
            .OrderByDescendingIf(selectedSort == nameof(AppStrings.Date), t => t.CreatedOn!)
            .ToList();
    }

    private bool WordIsVisible(WordDto word)
    {
        return string.IsNullOrWhiteSpace(searchText) || word.MainWord!.Contains(searchText!, StringComparison.OrdinalIgnoreCase);
    }

    private void SearchWords(string text)
    {
        searchText = text;

        FilterViewTodoItems();
    }

    private void SortTodoItems(BitDropdownItem<string> sort)
    {
        selectedSort = sort.Value;

        FilterViewTodoItems();
    }

    private void FilterTodoItems(string filter)
    {
        selectedFilter = filter;

        FilterViewTodoItems();
    }

    private void EditMode(WordDto word)
    {
        word.IsInEditMode = true;
        wordModel = word;
    }

    private async Task AddOrEdit()
    {
        if (isAdding) return;

        isAdding = true;

        try
        {
            if (wordModel.IsInEditMode is false)
            {
                var addedWord = await wordController.Create(wordModel, CurrentCancellationToken);

                allWords.Add(addedWord!);

                if (WordIsVisible(addedWord!))
                {
                    viewWords.Add(addedWord!);
                }
            }
            else
            {
                await UpdateWord(wordModel);
            }

            wordModel = new();
        }
        finally
        {
            isAdding = false;
        }
    }

    private async Task DeleteWord(WordDto word)
    {
        if (isLoading) return;

        try
        {
            var confirmed = await confirmMessageBox.Show(Localizer.GetString(nameof(AppStrings.AreYouSureWannaDelete), word.MainWord!),
                                                     Localizer[nameof(AppStrings.DeleteWord)]);

            if (confirmed)
            {
                isLoading = true;

                StateHasChanged();

                await wordController.Delete(word.Id, CurrentCancellationToken);

                allWords.Remove(word);

                viewWords.Remove(word);
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task CheckIfExist()
    {
        if (isLoading) return;

        try
        {
            if (wordModel.MainWord.HasNoValue())
            {
                await snackBar.Error(Localizer.GetString(nameof(AppStrings.Warning)), Localizer.GetString(nameof(AppStrings.EnterYourWord)));

                return;
            }

            var exist = allWords.Any(w => w.Id != wordModel.Id && w.MainWord!.Equals(wordModel.MainWord, StringComparison.OrdinalIgnoreCase));

            if (exist)
            {
                await snackBar.Warning(Localizer.GetString(nameof(AppStrings.Warning)), Localizer.GetString(nameof(AppStrings.ThisWordHasAlreadyBeenSaved)));
            }
            else
            {
                await snackBar.Success(Localizer.GetString(nameof(AppStrings.Success)), Localizer.GetString(nameof(AppStrings.YouCanAddThisWord)));
            }
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task UpdateWord(WordDto word)
    {
        (await wordController.Update(word, CurrentCancellationToken)).Patch(word);

        word.IsInEditMode = false;

        if (WordIsVisible(word) is false)
        {
            viewWords.Remove(word);
        }
    }

    protected override async ValueTask DisposeAsync(bool disposing)
    {
        await base.DisposeAsync(true);

        if (disposing)
        {
            await keyboard.DisposeAsync();
        }
    }
}
