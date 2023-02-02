using System.ComponentModel;
using Avalonia.Controls;
using ReactiveUI;

namespace English2k23.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    ViewModelBase content;

    public MainWindowViewModel()
    {
        Content =  new HomeViewModel();
    }

    public ViewModelBase Content
    {
        get => content;
        private set => this.RaiseAndSetIfChanged(ref content, value);
    }

    public void TestButtonClicked() {
        Content = new AnotherViewModel();
    }

    public void TestButtonClicked2() {
        Content = new AnotherViewModel2();
    }
}
