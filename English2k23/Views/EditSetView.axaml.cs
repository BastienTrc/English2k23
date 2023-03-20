using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class EditSetView : ReactiveUserControl<EditSetViewModel>
{
    public EditSetView()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: MainWindow mainWindow
            })
        {
            this.WhenActivated(disposables =>
            {
                disposables(ViewModel!.ShowDialogExist.RegisterHandler(mainWindow.DoShowDialogAsyncExistingQuest));
                disposables(ViewModel!.ShowDialogNew.RegisterHandler(mainWindow.DoShowDialogAsyncNewQuest));
                disposables(ViewModel!.ShowEditVideoDialog.RegisterHandler(mainWindow.DoShowDialogAsyncEditVideo));
            });
        }

        AvaloniaXamlLoader.Load(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}