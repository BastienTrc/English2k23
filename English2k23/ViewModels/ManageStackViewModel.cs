using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using DynamicData.Binding;
using English2k23.Models;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;


namespace English2k23.ViewModels;

public class ManageStackViewModel : ReactiveObject, IRoutableViewModel
{

    private bool _isEnabled;
    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    private Bitmap _imageToLoad;
    public Bitmap ImageToLoad
    {
        get => _imageToLoad;
        set => this.RaiseAndSetIfChanged(ref _imageToLoad, value);
    }

    private QuestionStack? _stackSelected;
    public QuestionStack? StackSelected
    {
        get => _stackSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _stackSelected, value);
            IsEnabled = _stackSelected is not null;
            LoadImage(value.PictureUrl);
        }
    }

    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    public ReactiveCommand<QuestionStack, IRoutableViewModel?> GoStackEdit { get; }
    public ReactiveCommand<QuestionStack, Unit> StackDeleted { get; }

    // Show open file dialog
    public ICommand OpenFileDialogCommand { get; }
    public Interaction<Unit, string?> ShowFileDialog { get; }

    public AvaloniaList<QuestionStack> QuestionStackList { get; }

    public ManageStackViewModel(IScreen hostScreen, Game game)
    {
        HostScreen = hostScreen;

        GoStackEdit = ReactiveCommand.CreateFromObservable<QuestionStack, IRoutableViewModel?>(
            stack => HostScreen.Router.Navigate.Execute(new EditStackViewModel(HostScreen, game, stack)));

        QuestionStackList = game.QuestionStacks;
        for (int i = 0; i < 25; i++)
        {
            QuestionStackList.Add(new QuestionStack("a"+i, "b"+(2*i), "c"+(3*i)));
        }

        StackDeleted = ReactiveCommand.Create<QuestionStack>(
            stack => QuestionStackList.Remove(stack)
        );

        // Handle add stack command
        ShowDialog = new Interaction<AddStackViewModel, QuestionStack?>();
        AddStackCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var addStackViewModel = new AddStackViewModel();

            var result = await ShowDialog.Handle(addStackViewModel);
            if (result != null)
            {
                QuestionStackList.Add(result);
            }

        });

        ShowFileDialog = new Interaction<Unit, string?>();
        OpenFileDialogCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            // No need to create and give a viewModel as OpenFileDialog don't need one
            var result = await ShowFileDialog.Handle(new Unit());

            // If PictureURL wasn't specified then it contains "null"
            if (StackSelected != null) StackSelected.PictureUrl = result;
            LoadImage(result);
        });
    }

    // Handle add stack command
    public ICommand AddStackCommand { get; }
    public Interaction<AddStackViewModel, QuestionStack?> ShowDialog { get; }


    private void LoadImage(string? pictureUrl)
    {
        try
        {
            ImageToLoad = new Bitmap(pictureUrl) ;
        }
        catch (Exception e)
        {
            ImageToLoad = new Bitmap("/Users/bastienturco/Desktop/English2k23/English2k23/bin/Debug/net7.0/Pictures/default.png");
        }

    }

}