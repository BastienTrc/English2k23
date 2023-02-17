using Avalonia.Collections;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class EditStackViewModel : ReactiveObject, IRoutableViewModel
{
    private Game game;
    private Question _selectedQuestion;

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    private QuestionStack QuestionStack { get; }
    public AvaloniaList<Question> QuestionList { get; }

    public Question SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedQuestion, value);
            Console.WriteLine(_selectedQuestion.Expression);
        }
    }

    public EditStackViewModel(IScreen hostScreen, Game game, QuestionStack questionStack)
    {
        this.game = game;
        HostScreen = hostScreen;
        QuestionStack = questionStack;
        QuestionList = questionStack.getQuestions();

        for (int i = 0; i < 15; i++)
        {
            QuestionList.Add(new Question("expr" + i, "def", "1 ; 2 ; 3", "path", false));
        }

        _selectedQuestion = QuestionList[0];
    }
}