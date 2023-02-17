using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.Models;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

#if DEBUG
        this.AttachDevTools();
#endif
    }

    public async Task DoShowDialogAsync(InteractionContext<AddStackViewModel, QuestionStack?> interaction)
    {
        var dialog = new AddStackWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<QuestionStack?>(this);
        interaction.SetOutput(result);
    }
}