using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class EditStackView : ReactiveUserControl<EditStackViewModel>
{
    public EditStackView()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: MainWindow mainWindow
            })
        {
            this.WhenActivated(disposables =>
            {
                disposables(ViewModel!.ShowDialogExist.RegisterHandler(mainWindow.DoShowDialogAsyncExistingQuest));
            });
            this.WhenActivated(disposables =>
            {
                disposables(ViewModel!.ShowDialogNew.RegisterHandler(mainWindow.DoShowDialogAsyncNewQuest));
            });
        }
        AvaloniaXamlLoader.Load(this);
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}