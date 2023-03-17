using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Data;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class AddStackViewModel : ViewModelBase
{
    private bool _isEnabled;
    private string? _pictureUrl;
    private string? _stackDescription;
    private string? _stackName;

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

        Validate = ReactiveCommand.Create<string, QuestionStack?>(cancelToken =>
            cancelToken == "Cancel" ? null : new QuestionStack(_stackName!, _stackDescription!, PictureUrl));
    }

    // Show open dialog file
    public ICommand OpenFileDialogCommand { get; }
    public Interaction<Unit, string?> ShowDialog { get; }

    // Validate Fields and create questionStack
    public ReactiveCommand<string, QuestionStack?> Validate { get; }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public string? StackName
    {
        get => _stackName;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(StackName) || string.IsNullOrWhiteSpace(StackDescription));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Name field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _stackName, value);
        }
    }

    public string? StackDescription
    {
        get => _stackDescription;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(StackName) || string.IsNullOrWhiteSpace(StackDescription));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Description field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _stackDescription, value);
        }
    }

    public string? PictureUrl
    {
        get => _pictureUrl;
        set => this.RaiseAndSetIfChanged(ref _pictureUrl, value ?? "null");
    }
}