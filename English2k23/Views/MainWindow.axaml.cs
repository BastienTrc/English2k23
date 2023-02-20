using System.Reactive;
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
        dialog.Title = "Create a set of questions!";
        dialog.Opacity = 0.75;


        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<QuestionStack?>(this);
        interaction.SetOutput(result);
    }

    public async Task DoShowDialogAsyncExistingQuest(
        InteractionContext<AddExistingQuestionViewModel, AvaloniaList<Question>?> interaction)
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

    public async Task DoShowFileDialogAsync(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFileDialog();

        dialog.AllowMultiple = false;
        dialog.Title = "Select a picture!";
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + "Pictures/";
        Console.WriteLine(dialog.Directory);
        dialog.Filters.Add(
            new FileDialogFilter { Extensions = { "png", "jpg", "jpeg" } }
        );

        var result = await dialog.ShowAsync(this);
        interaction.SetOutput(result != null ? result.FirstOrDefault() : "null");
    }
}
