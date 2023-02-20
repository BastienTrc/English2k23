using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class AddExistingQuestionWindow : ReactiveWindow<AddExistingQuestionViewModel>
{
    public AddExistingQuestionWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.Validate.Subscribe(Close)));
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}