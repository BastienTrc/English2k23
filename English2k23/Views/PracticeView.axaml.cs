using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class PracticeView : ReactiveUserControl<PracticeViewModel>
{
    public PracticeView()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            {
                MainWindow: MainWindow mainWindow
            })
            this.WhenActivated(disposable =>
            {
                disposable(ViewModel!.ShowVideoDialog.RegisterHandler(mainWindow.DoShowVideoPlayerAsync));
            });

        AvaloniaXamlLoader.Load(this);
    }
}