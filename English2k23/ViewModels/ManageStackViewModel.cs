using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Text.Json;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class ManageStackViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly Bitmap? _default;

    private Bitmap? _imageToLoad;

    private bool _isEnabled;

    private QuestionStack? _stackSelected;

    public ManageStackViewModel(IScreen hostScreen, Game game)
    {
        HostScreen = hostScreen;

        GoStackEdit = ReactiveCommand.CreateFromObservable<QuestionStack, IRoutableViewModel?>(
            stack => HostScreen.Router.Navigate.Execute(new EditStackViewModel(HostScreen, game, stack)));

        QuestionStackList = game.QuestionStacks;

        StackDeleted = ReactiveCommand.Create<QuestionStack>(
            stack => QuestionStackList.Remove(stack)
        );

        // Handle add stack command
        ShowAddStackDialog = new Interaction<AddStackViewModel, QuestionStack?>();
        AddStackCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addStackViewModel = new AddStackViewModel();

            var result = await ShowAddStackDialog.Handle(addStackViewModel);
            if (result != null) QuestionStackList.Add(result);
        });

        ShowEditPictureDialog = new Interaction<Unit, string?>();
        EditPictureCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // No need to create and give a viewModel as OpenFileDialog don't need one
            var result = await ShowEditPictureDialog.Handle(new Unit());

            // If PictureURL wasn't specified then it contains "null"
            if (StackSelected != null) StackSelected.PictureUrl = result;
            LoadImage(result);
        });
        
        ShowSaveDialog = new Interaction<Unit, String?>();
        SaveStackCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await ShowSaveDialog.Handle(new Unit());

                if (StackSelected != null && result != null)
                {
                    var saveStack = new SaveStack(StackSelected);
                    var jsonString = JsonSerializer.Serialize(saveStack);
                    File.WriteAllText(result, jsonString);
                }
            }
        );
        
        ShowLoadDialog = new Interaction<Unit, String?>();
        LoadStackCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await ShowLoadDialog.Handle(new Unit());

                if (result != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        IncludeFields = true,
                    };
                    var jsonString = File.ReadAllText(result);
                    var save = JsonSerializer.Deserialize<SaveStack>(jsonString,options)!;
                    var stack = new QuestionStack(save.Name, save.Description, save.PictureUrl);
                    game.AddStack(stack);
                    save.questionList.ForEach(quest=>game.AddQuestionToStack(stack,quest));
                }
            }
        );

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        try
        {
            _default = new Bitmap(assets?.Open(new Uri(@"avares://English2k23/Assets/icons/default.png")));
        }
        catch
        {
            _default = new Bitmap(@"../../../Assets/icons/default.png");
        }
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public Bitmap? ImageToLoad
    {
        get => _imageToLoad;
        set => this.RaiseAndSetIfChanged(ref _imageToLoad, value);
    }

    public QuestionStack? StackSelected
    {
        get => _stackSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _stackSelected, value);
            IsEnabled = _stackSelected is not null;
            LoadImage(value?.PictureUrl);
        }
    }

    public ReactiveCommand<QuestionStack, IRoutableViewModel?> GoStackEdit { get; }
    public ReactiveCommand<QuestionStack, Unit> StackDeleted { get; }

    // Show open file dialog
    public ICommand EditPictureCommand { get; }
    public Interaction<Unit, string?> ShowEditPictureDialog { get; }

    public AvaloniaList<QuestionStack> QuestionStackList { get; }

    // Save dialog
    public ICommand SaveStackCommand { get; }
    public Interaction<Unit, string?> ShowSaveDialog { get; }
    
    // Load dialog
    public ICommand LoadStackCommand { get; }
    public Interaction<Unit, string?> ShowLoadDialog { get; }
    
    // Handle add stack command
    public ICommand AddStackCommand { get; }
    public Interaction<AddStackViewModel, QuestionStack?> ShowAddStackDialog { get; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }


    private void LoadImage(string? pictureUrl)
    {
        try
        {
            ImageToLoad = new Bitmap(
                $"{AppDomain.CurrentDomain.BaseDirectory}Pictures{Path.DirectorySeparatorChar}{pictureUrl}");
        }
        catch (Exception)
        {
            ImageToLoad = _default;
        }
    }
}