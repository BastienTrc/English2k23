using Avalonia;
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
            this.WhenActivated(disposable =>
            {
                disposable(ViewModel!.ShowAddStackDialog.RegisterHandler(mainWindow.DoShowDialogAsync));
                disposable(ViewModel!.ShowEditPictureDialog.RegisterHandler(mainWindow.DoShowDialogAsyncEditPicture));
            });

        AvaloniaXamlLoader.Load(this);
    }
}