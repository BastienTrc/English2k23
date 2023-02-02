using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace English2k23.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}