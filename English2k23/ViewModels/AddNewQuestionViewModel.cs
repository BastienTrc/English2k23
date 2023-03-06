using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class AddNewQuestionViewModel : ViewModelBase
{
    private string? VideoUrl { get; set; }
    private string? _questionExpression;
    private string? _questionDefinition;
    private string? _questionAnswers;
    
    public ICommand OpenFileDialogCommand { get; }
    public ReactiveCommand<Unit, Question?> Validate { get; }
    
    public string? QuestionExpression
    {
        get => _questionExpression;
        set
        {
            this.RaiseAndSetIfChanged(ref _questionExpression, value);
        }
    }
    
    public string? QuestionDefinition
    {
        get => _questionDefinition;
        set
        {
            this.RaiseAndSetIfChanged(ref _questionDefinition, value);
        }
    }
    
    public string? QuestionAnswers
    {
        get => _questionAnswers;
        set
        {
            this.RaiseAndSetIfChanged(ref _questionAnswers, value);
        }
    }
    
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
            if (_questionExpression is null || _questionDefinition is null|| _questionAnswers is null)
            {
                return null;
            }
            
            if (VideoUrl == null)
            {
                return new Question(_questionExpression, _questionDefinition, _questionAnswers,"",false);
            }
            return new Question(_questionExpression, _questionDefinition, _questionAnswers,VideoUrl,true);
            });
    }

    public Interaction<Unit, string?> ShowDialog { get; }
}