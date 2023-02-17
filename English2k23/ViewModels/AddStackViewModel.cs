using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class AddStackViewModel : ViewModelBase
{
    private string? PictureUrl { get; set; }
    private string _stackName;
    private string _stackDescription;

    public ICommand OpenFileDialogCommand { get; }
    public ReactiveCommand<Unit, QuestionStack?> Validate { get; }

    public string StackDescription
    {
        get => _stackDescription;
        set => this.RaiseAndSetIfChanged(ref _stackName, value);
    }

    public string StackName
    {
        get => _stackName;
        set => this.RaiseAndSetIfChanged(ref _stackDescription, value);
    }

    public AddStackViewModel()
    {
        ShowDialog = new Interaction<Unit, string?>();

        OpenFileDialogCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // No need to create and give a viewModel as OpenFileDialog don't need one
            var result = await ShowDialog.Handle(new Unit());

            // If PictureURL wasn't specified then it contains "null"
            PictureUrl = result;
        });

        Validate = ReactiveCommand.Create(() =>
        {
            if (_stackName is null || _stackDescription is null)
            {
                return null;
            }

            return new QuestionStack(_stackName, _stackDescription, PictureUrl);
        });
    }

    public Interaction<Unit, string?> ShowDialog { get; }
}