using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class PracticeView : ReactiveUserControl<PracticeViewModel>
{
    public PracticeView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}