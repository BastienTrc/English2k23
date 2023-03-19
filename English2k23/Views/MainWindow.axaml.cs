using System.Reactive;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
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

    public async Task DoShowDialogAsyncEditPicture(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFileDialog();

        dialog.AllowMultiple = false;
        dialog.Title = "Select a picture!";
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + $"Pictures{Path.DirectorySeparatorChar}";
        Console.WriteLine(dialog.Directory);
        dialog.Filters?.Add(
            new FileDialogFilter { Extensions = { "png", "jpg", "jpeg" } }
        );

        var result = await dialog.ShowAsync(this);
        if (result != null && result.Length == 0)
        {
            interaction.SetOutput("");
            return;
        }

        var splittedRes = result?[0].Split($"Pictures{Path.DirectorySeparatorChar}");
        Console.WriteLine(result?[0]);

        if (splittedRes is null || splittedRes.Length < 2)
            interaction.SetOutput("");
        else
            interaction.SetOutput(splittedRes[1]);
    }
    
    public async Task DoShowDialogAsyncEditVideo(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFileDialog();

        dialog.AllowMultiple = false;
        dialog.Title = "Select a video!";
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + $"Videos{Path.DirectorySeparatorChar}";
        Console.WriteLine(dialog.Directory);
        dialog.Filters?.Add(
            new FileDialogFilter { Extensions = { "mp4" } }
        );

        var result = await dialog.ShowAsync(this);
        if (result != null && result.Length == 0)
        {
            interaction.SetOutput("");
            return;
        }

        var splittedRes = result?[0].Split($"Videos{Path.DirectorySeparatorChar}");
        Console.WriteLine(result?[0]);

        if (splittedRes is null || splittedRes.Length < 2)
            interaction.SetOutput("");
        else
            interaction.SetOutput(splittedRes[1]);
    }
    
    public async Task DoShowSaveDialogAsync(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new SaveFileDialog();
        
        dialog.Title = "Save as";
        dialog.DefaultExtension="json";
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + "Set" + Path.DirectorySeparatorChar;

        var result = await dialog.ShowAsync(this);
        interaction.SetOutput(result);
    }
    
    public async Task DoShowLoadDialogAsync(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFileDialog();

        dialog.AllowMultiple = false;
        dialog.Title = "Select a save file!";
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + "Set"+ Path.DirectorySeparatorChar;
        Console.WriteLine(dialog.Directory);
        dialog.Filters.Add(
            new FileDialogFilter { Extensions = { "json" } }
        );

        var result = await dialog.ShowAsync(this);
        interaction.SetOutput(result?.FirstOrDefault());
    }

    public async Task DoShowVideoPlayerAsync(InteractionContext<VideoPlayerModel, Unit> interaction)
    {
        var dialog = new VideoPlayerWindow
        {
            Title = "Video hint!",
            Opacity = 0.75,
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<Unit>(this);
        interaction.SetOutput(result);
    }
}