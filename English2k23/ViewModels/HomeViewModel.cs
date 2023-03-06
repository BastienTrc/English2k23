
using System.Reactive;

using English2k23.Models;

using ReactiveUI;

namespace English2k23.ViewModels;

public class HomeViewModel : ReactiveObject, IRoutableViewModel
{
    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ReactiveCommand<Unit, IRoutableViewModel> GoEdit { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> GoPractice { get; }

    public HomeViewModel(IScreen screen, Game game)
    {
        HostScreen = screen;
        GoEdit = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new ManageStackViewModel(HostScreen, game))
        );

        GoPractice = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new TrainViewModel(HostScreen, game))
        );
    }
}