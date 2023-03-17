using System.Reactive;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class AddStackWindow : ReactiveWindow<AddStackViewModel>
{
    public AddStackWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.Validate.Subscribe(Close)));
        this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }

    private async Task DoShowDialogAsync(InteractionContext<Unit, string?> interaction)
    {
        var dialog = new OpenFileDialog();

        dialog.AllowMultiple = false;
        dialog.Title = "Select a picture!";
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + $"Pictures{Path.DirectorySeparatorChar}";
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

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}