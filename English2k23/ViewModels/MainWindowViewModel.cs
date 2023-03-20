using System.Reactive;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    public MainWindowViewModel()
    {
        var game = new Game(Guid.NewGuid(), "MyGame", "Hello");
        // Manage the routing state. Use the Router.Navigate.Execute
        // command to navigate to different view models.
        //
        // Note, that the Navigate.Execute method accepts an instance
        // of a view model, this allows you to pass parameters to
        // your view models, or to reuse existing view models.

        GoEdit = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new ManageSetViewModel(this, game))
        );
        GoPractice = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new TrainViewModel(this, game))
        );
    }

    // The command that navigates a user to first view model.
    public ReactiveCommand<Unit, IRoutableViewModel> GoEdit { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> GoPractice { get; }
    public RoutingState Router { get; } = new();

    // The command that navigates a user back.

    public void GoBack()
    {
        if (Router.NavigationStack.Count < 1) return;
        Router.NavigateBack.Execute();
    }

    public void GoHome()
    {
        Router.NavigationStack.Clear();
    }
}