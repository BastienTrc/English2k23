using System.ComponentModel;
using Avalonia.Controls;
using ReactiveUI;

namespace English2k23.ViewModels;

public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    public string Greeting => "Welcome to Avalonia!";
    public string GreetingAgain => "Welcome to Avalonia again!";

    public new event PropertyChangedEventHandler? PropertyChanged;

    private string _buttonText = "Click me!";
    public string ButtonText
    {
        get => _buttonText;
        set
        {
            _buttonText = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText)));
        }
    }

    public void TestButtonClicked() => ButtonText = "Hello you!";

    private string _buttonText2 = "Click me!";
    public string ButtonText2
    {
        get => _buttonText2;
        set
        {
            _buttonText2 = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ButtonText2)));
        }
    }

    public string Title => "Welcome to Engligh2k23";

    public void TestButtonClicked2() => ButtonText2 = "Hello you!";
}
