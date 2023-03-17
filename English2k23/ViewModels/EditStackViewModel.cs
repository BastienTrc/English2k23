using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class EditStackViewModel : ReactiveObject, IRoutableViewModel
{
    private Question? _selectedQuestion;

    public EditStackViewModel(IScreen hostScreen, Game game, QuestionStack questionStack)
    {
        HostScreen = hostScreen;
        QuestionList = questionStack.getQuestions();

        for (var i = 0; i < 4; i++)
            QuestionList.Add(new Question("expr" + i, "def" + i, $"{i}_1; {i}_2 ; {i}_3; {i}_4", "path", false));

        _selectedQuestion = QuestionList[0];

        QuestionDeleted = ReactiveCommand.Create<Question>(
            quest => game.RemoveQuestionFromStack(questionStack, quest));

        //Handle add existing question command
        ShowDialogExist = new Interaction<AddExistingQuestionViewModel, AvaloniaList<Question>?>();

        AddExistingQuestionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addExistingQuestionViewModel = new AddExistingQuestionViewModel(game);

            var result = await ShowDialogExist.Handle(addExistingQuestionViewModel);
            if (result != null)
                foreach (var quest in result)
                    if (!questionStack.getQuestions().Contains(quest))
                        game.AddQuestionToStack(questionStack, quest);
        });

        //Handle add new question command
        ShowDialogNew = new Interaction<AddNewQuestionViewModel, Question?>();

        AddNewQuestionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addNewQuestionViewModel = new AddNewQuestionViewModel();

            var result = await ShowDialogNew.Handle(addNewQuestionViewModel);
            if (result != null)
            {
                game.AddQuestionToGame(result);
                game.AddQuestionToStack(questionStack, result);
            }
        });
    }

    public ReactiveCommand<Question, Unit> QuestionDeleted { get; }
    public AvaloniaList<Question?> QuestionList { get; }

    public Question? SelectedQuestion
    {
        get => _selectedQuestion;
        set => this.RaiseAndSetIfChanged(ref _selectedQuestion, value);
    }

    // Handle add existing question command
    public ICommand AddExistingQuestionCommand { get; }

    public Interaction<AddExistingQuestionViewModel, AvaloniaList<Question>?> ShowDialogExist { get; }

    // Handle add new question command
    public ICommand AddNewQuestionCommand { get; }

    public Interaction<AddNewQuestionViewModel, Question?> ShowDialogNew { get; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
}