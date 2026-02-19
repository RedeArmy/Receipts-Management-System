using CommunityToolkit.Mvvm.ComponentModel;

namespace ReceiptsManagementSystem.Presentation.ViewModels.Base;

public abstract class BaseViewModel : ObservableObject
{
    private bool _isBusy;
    private string _title = string.Empty;

    private bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    protected async Task ExecuteBusyAsync(Func<Task> operation)
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await operation();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
