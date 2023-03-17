using Avalonia;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class TrainViewModel : ReactiveObject, IRoutableViewModel
{
    private readonly Bitmap? _default;

    private Bitmap? _imageToLoad;

    private bool _isEnabled;

    private bool _showWarning;

    private QuestionStack? _stackSelected;

    private string? _warningMessage;

    public TrainViewModel(IScreen hostScreen, Game game)
    {
        HostScreen = hostScreen;

        QuestionStackList = game.QuestionStacks;

        GoPractice = ReactiveCommand.CreateFromObservable<QuestionStack, IRoutableViewModel>(stack =>
            HostScreen.Router.Navigate.Execute(new PracticeViewModel(HostScreen, stack)));

        GoCompetitive = ReactiveCommand.CreateFromObservable<QuestionStack, IRoutableViewModel>(stack =>
            HostScreen.Router.Navigate.Execute(new CompetitiveViewModel(HostScreen, game, stack)));

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

    public string? WarningMessage
    {
        get => _warningMessage;
        set => this.RaiseAndSetIfChanged(ref _warningMessage, value);
    }

    public bool ShowWarning
    {
        get => _showWarning;
        set => this.RaiseAndSetIfChanged(ref _showWarning, value);
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
            if (_stackSelected?.getQuestions().Count == 0)
            {
                IsEnabled = false;
                ShowWarning = true;
                WarningMessage = "Selected set has no question in it";
            }
            else if (_stackSelected?.getQuestions().Count < 5)
            {
                ShowWarning = true;
                WarningMessage = "Selected set has less than 5 questions in it";
            }
            else
            {
                ShowWarning = false;
            }

            LoadImage(value?.PictureUrl);
        }
    }

    // Show open file dialog

    public AvaloniaList<QuestionStack> QuestionStackList { get; }
    public ReactiveCommand<QuestionStack, IRoutableViewModel> GoPractice { get; }
    public ReactiveCommand<QuestionStack, IRoutableViewModel> GoCompetitive { get; }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }


    private void LoadImage(string? pictureUrl)
    {
        try
        {
            ImageToLoad = new Bitmap(pictureUrl);
        }
        catch (Exception)
        {
            ImageToLoad = _default;
        }
    }
}