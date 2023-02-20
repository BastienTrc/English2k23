using Avalonia.Collections;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Reactive;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class EditStackViewModel : ReactiveObject, IRoutableViewModel
{
    private Game game;
    private Question _selectedQuestion;

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }
    public ReactiveCommand<Question, Unit> QuestionDeleted { get; }
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

        QuestionDeleted = ReactiveCommand.Create<Question>(
            quest => game.RemoveQuestionFromStack(questionStack,quest));
        
        //Handle add existing question command
        ShowDialogExist = new Interaction<AddExistingQuestionViewModel, AvaloniaList<Question>?>();

        AddExistingQuestionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addExistingQuestionViewModel = new AddExistingQuestionViewModel(game);

            var result = await ShowDialogExist.Handle(addExistingQuestionViewModel);
            if (result != null)
            {
                foreach (var quest in result)
                {
                    if (!questionStack.getQuestions().Contains(quest))
                    {
                        game.AddQuestionToStack(questionStack,quest);
                    }
                }
            }
            else
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("title", "Error occured");
                messageBoxStandardWindow.Show();
            }
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
                game.AddQuestionToStack(questionStack,result);
            }
            else
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("title", "Error occured");
                messageBoxStandardWindow.Show();
            }
        });
    }
    
    // Handle add existing question command
    public ICommand AddExistingQuestionCommand { get; }

    public Interaction<AddExistingQuestionViewModel, AvaloniaList<Question>?> ShowDialogExist { get; }
    
    // Handle add new question command
    public ICommand AddNewQuestionCommand { get; }

    public Interaction<AddNewQuestionViewModel, Question?> ShowDialogNew { get; }
}