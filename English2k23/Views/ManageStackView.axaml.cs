using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class ManageStackView : ReactiveUserControl<ManageStackViewModel>
{
    public ManageStackView()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: MainWindow mainWindow
            })
        {
            this.WhenActivated(disposable =>
            {
                disposable(ViewModel!.ShowDialog.RegisterHandler(mainWindow.DoShowDialogAsync));
                disposable(ViewModel!.ShowFileDialog.RegisterHandler(mainWindow.DoShowFileDialogAsync));
            });

        }

        AvaloniaXamlLoader.Load(this);
    }
}