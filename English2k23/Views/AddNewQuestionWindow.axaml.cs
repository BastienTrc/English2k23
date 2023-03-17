using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class AddNewQuestionWindow : ReactiveWindow<AddNewQuestionViewModel>
{
    public AddNewQuestionWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.Validate.Subscribe(Close)));
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private async Task DoShowDialogAsync(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFileDialog();
        dialog.AllowMultiple = false;
        dialog.Title = "Select a video!";
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + $"Videos{Path.DirectorySeparatorChar}";
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

        if (splittedRes == null || splittedRes.Length < 2)
            interaction.SetOutput("");
        else
            interaction.SetOutput(splittedRes[1]);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}