using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class EditStackView : ReactiveUserControl<EditStackViewModel>
{
    public EditStackView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}