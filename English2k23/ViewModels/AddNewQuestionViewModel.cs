using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Data;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class AddNewQuestionViewModel : ViewModelBase
{
    private bool _isEnabled;
    private string? _firstChoice;
    private string? _secondChoice;
    private string? _thirdChoice;
    private string? _questionDefinition;
    private string? _questionExpression;
    private string? _videoUrl;

    public AddNewQuestionViewModel()
    {
        ShowDialog = new Interaction<Unit, string?>();

        OpenFileDialogCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // No need to create and give a viewModel as OpenFileDialog don't need one
            var result = await ShowDialog.Handle(new Unit());

            // If PictureURL wasn't specified then it contains "null"
            VideoUrl = result;
        });

        Validate = ReactiveCommand.Create(() =>
        {
            if (QuestionExpression is null || QuestionDefinition is null || FirstChoice is null || SecondChoice is null || ThirdChoice is null) return null;

            if (string.IsNullOrWhiteSpace(VideoUrl))
                return new Question(QuestionExpression, QuestionDefinition,$"{FirstChoice};{SecondChoice};{ThirdChoice}", "", false);
            return new Question(QuestionExpression, QuestionDefinition,$"{FirstChoice};{SecondChoice};{ThirdChoice}", VideoUrl, true);
        });
    }

    public ICommand OpenFileDialogCommand { get; }
    public ReactiveCommand<Unit, Question?> Validate { get; }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public string? QuestionExpression
    {
        get => _questionExpression;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(value) ||
                          string.IsNullOrWhiteSpace(QuestionDefinition) ||
                          string.IsNullOrWhiteSpace(FirstChoice) ||
                          string.IsNullOrWhiteSpace(SecondChoice) ||
                          string.IsNullOrWhiteSpace(ThirdChoice));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Expression field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _questionExpression, value);
        }
    }

    public string? QuestionDefinition
    {
        get => _questionDefinition;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(QuestionExpression) ||
                          string.IsNullOrWhiteSpace(value) ||
                          string.IsNullOrWhiteSpace(FirstChoice) ||
                          string.IsNullOrWhiteSpace(SecondChoice) ||
                          string.IsNullOrWhiteSpace(ThirdChoice));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Definition field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _questionDefinition, value);
        }
    }

    public string? FirstChoice
    {
        get => _firstChoice;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(QuestionExpression) ||
                          string.IsNullOrWhiteSpace(QuestionDefinition) ||
                          string.IsNullOrWhiteSpace(value) ||
                          string.IsNullOrWhiteSpace(SecondChoice) ||
                          string.IsNullOrWhiteSpace(ThirdChoice));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Answers field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _firstChoice, value);
        }
    }
    
    public string? SecondChoice
    {
        get => _secondChoice;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(QuestionExpression) ||
                          string.IsNullOrWhiteSpace(QuestionDefinition) ||
                          string.IsNullOrWhiteSpace(FirstChoice) ||
                          string.IsNullOrWhiteSpace(value) ||
                          string.IsNullOrWhiteSpace(ThirdChoice));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Answers field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _secondChoice, value);
        }
    }
    
    public string? ThirdChoice
    {
        get => _thirdChoice;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(QuestionExpression) ||
                          string.IsNullOrWhiteSpace(QuestionDefinition) ||
                          string.IsNullOrWhiteSpace(FirstChoice) ||
                          string.IsNullOrWhiteSpace(SecondChoice) ||
                          string.IsNullOrWhiteSpace(value));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Answers field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _thirdChoice, value);
        }
    }

    public string? VideoUrl
    {
        get => _videoUrl;
        set => this.RaiseAndSetIfChanged(ref _videoUrl, value);
    }

    public Interaction<Unit, string?> ShowDialog { get; }
}