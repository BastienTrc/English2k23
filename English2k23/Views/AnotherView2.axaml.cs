using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace English2k23.Views;

public partial class AnotherView2 : UserControl
{
    public AnotherView2()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}