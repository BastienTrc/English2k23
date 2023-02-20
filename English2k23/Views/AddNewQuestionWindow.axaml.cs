using Avalonia;
using System.Reactive;
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
        dialog.Filters.Add(
            new FileDialogFilter { Extensions = { "mp4" } }
        );

        var result = await dialog.ShowAsync(this);
        if (result != null) interaction.SetOutput(result.FirstOrDefault());
        else interaction.SetOutput("null");
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}