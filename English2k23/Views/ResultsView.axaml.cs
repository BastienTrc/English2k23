using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using English2k23.ViewModels;
using ReactiveUI;

namespace English2k23.Views;

public partial class ResultsView : ReactiveUserControl<ResultsViewModel>
{
    public ResultsView()
    {
        InitializeComponent();
        AvaloniaXamlLoader.Load(this);
    }

}