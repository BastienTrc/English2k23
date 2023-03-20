using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class EditSetViewModel : ReactiveObject, IRoutableViewModel
{
    private Question? _selectedQuestion;

    public EditSetViewModel(IScreen hostScreen, Game game, QuestionSet questionSet)
    {
        HostScreen = hostScreen;
        QuestionList = questionSet.getQuestions();

        if (QuestionList.Count != 0)
        {
            SelectedQuestion = QuestionList[0];
        }

        QuestionDeleted = ReactiveCommand.Create<Question>(
            quest => game.RemoveQuestionFromStack(questionSet, quest));

        //Handle add existing question command
        ShowDialogExist = new Interaction<AddExistingQuestionViewModel, AvaloniaList<Question>?>();

        AddExistingQuestionCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addExistingQuestionViewModel = new AddExistingQuestionViewModel(game);

            var result = await ShowDialogExist.Handle(addExistingQuestionViewModel);
            if (result != null)
                foreach (var quest in result)
                    if (!questionSet.getQuestions().Contains(quest))
                        game.AddQuestionToStack(questionSet, quest);
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
                game.AddQuestionToStack(questionSet, result);
            }
        });
        
        ShowEditVideoDialog = new Interaction<Unit, string?>();
        EditVideoCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // No need to create and give a viewModel as OpenFileDialog don't need one
            var result = await ShowEditVideoDialog.Handle(new Unit()) ?? "";

            // If PathToVideo wasn't specified then it contains "null"
            if (SelectedQuestion != null && !string.IsNullOrEmpty(result))
            {
                SelectedQuestion.PathToVideo = result;
                SelectedQuestion.VideoMode = true;
            }
        });

        VideoDeleted = ReactiveCommand.Create<Question>(
            quest =>
            {
                quest.PathToVideo = "";
                quest.VideoMode = false;
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
     
     public ICommand EditVideoCommand { get; }
     public Interaction<Unit, string?> ShowEditVideoDialog { get; }
     
     public ReactiveCommand<Question, Unit> VideoDeleted { get; }
 
     public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
     public IScreen HostScreen { get; }
 }