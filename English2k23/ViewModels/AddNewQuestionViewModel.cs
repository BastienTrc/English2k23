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
    private string? _questionAnswers;
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
            if (_questionExpression is null || _questionDefinition is null || _questionAnswers is null) return null;

            if (string.IsNullOrWhiteSpace(VideoUrl))
                return new Question(_questionExpression, _questionDefinition, _questionAnswers, "", false);
            return new Question(_questionExpression, _questionDefinition, _questionAnswers, VideoUrl, true);
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
            IsEnabled = !(string.IsNullOrWhiteSpace(_questionExpression) ||
                          string.IsNullOrWhiteSpace(_questionDefinition) ||
                          string.IsNullOrWhiteSpace(_questionAnswers));
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
            IsEnabled = !(string.IsNullOrWhiteSpace(_questionExpression) ||
                          string.IsNullOrWhiteSpace(_questionDefinition) ||
                          string.IsNullOrWhiteSpace(_questionAnswers));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Definition field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _questionDefinition, value);
        }
    }

    public string? QuestionAnswers
    {
        get => _questionAnswers;
        set
        {
            IsEnabled = !(string.IsNullOrWhiteSpace(_questionExpression) ||
                          string.IsNullOrWhiteSpace(_questionDefinition) ||
                          string.IsNullOrWhiteSpace(_questionAnswers));
            if (string.IsNullOrWhiteSpace(value))
            {
                IsEnabled = false;
                throw new DataValidationException("Answers field can't be empty");
            }

            this.RaiseAndSetIfChanged(ref _questionAnswers, value);
        }
    }

    public string? VideoUrl
    {
        get => _videoUrl;
        set => this.RaiseAndSetIfChanged(ref _videoUrl, value);
    }

    public Interaction<Unit, string?> ShowDialog { get; }
}