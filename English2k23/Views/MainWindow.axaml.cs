using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Collections;
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
    
    public async Task DoShowDialogAsyncExistingQuest(InteractionContext<AddExistingQuestionViewModel, AvaloniaList<Question>?> interaction)
    {
        var dialog = new AddExistingQuestionWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<AvaloniaList<Question>>(this);
        interaction.SetOutput(result);
    }
    
    public async Task DoShowDialogAsyncNewQuest(InteractionContext<AddNewQuestionViewModel, Question?> interaction)
    {
        var dialog = new AddNewQuestionWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<Question>(this);
        interaction.SetOutput(result);
    }
}