using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;
using Avalonia.Diagnostics;
using Microsoft.VisualBasic;

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
        dialog.Directory = AppDomain.CurrentDomain.BaseDirectory + "Pictures/";
        Console.WriteLine(dialog.Directory);
        dialog.Filters.Add(
            new FileDialogFilter { Extensions = { "png", "jpg", "jpeg" } }
        );

        var result = await dialog.ShowAsync(this);
        interaction.SetOutput(result != null ? result.FirstOrDefault() : "null");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}