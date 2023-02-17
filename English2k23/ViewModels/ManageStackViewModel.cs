using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Controls;
using English2k23.Models;
using ReactiveUI;


namespace English2k23.ViewModels;

public class ManageStackViewModel : ReactiveObject, IRoutableViewModel
{
    private bool _isSelected;
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    public ReactiveCommand<QuestionStack, IRoutableViewModel?> GoStackEdit { get; }
    public ReactiveCommand<QuestionStack, Unit> StackDeleted { get; }

    public AvaloniaList<QuestionStack> QuestionStackList { get; }

    public ManageStackViewModel(IScreen hostScreen, Game game)
    {
        HostScreen = hostScreen;


        GoStackEdit = ReactiveCommand.CreateFromObservable<QuestionStack, IRoutableViewModel?>(
            stack => HostScreen.Router.Navigate.Execute(new EditStackViewModel(HostScreen, game, stack)));


        QuestionStackList = game.QuestionStacks;
        for (int i = 0; i < 25; i++)
        {
            QuestionStackList.Add(new QuestionStack("a", "b", "c"));
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
            else
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow("title", "Error occured");
                messageBoxStandardWindow.Show();
            }
        });
    }

    // Handle add stack command
    public ICommand AddStackCommand { get; }

    public Interaction<AddStackViewModel, QuestionStack?> ShowDialog { get; }
}