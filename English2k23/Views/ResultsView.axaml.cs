using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;

namespace English2k23.Views;

public partial class ResultsView : ReactiveUserControl<ResultsViewModel>
{
    public ResultsView()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
    }
}