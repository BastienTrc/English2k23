using English2k23.ViewModels;
using English2k23.Views;
using ReactiveUI;

namespace English2k23;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor ResolveView<T>(T viewModel, string? contract = null) => viewModel switch
    {
        HomeViewModel context => new HomeView { DataContext = context },
        ManageStackViewModel context => new ManageStackView { DataContext = context },
        TrainViewModel context => new TrainView { DataContext = context },
        EditStackViewModel context => new EditStackView { DataContext = context },
        PracticeViewModel context => new PracticeView { DataContext = context },
        CompetitiveViewModel context => new CompetitiveView { DataContext = context },
        ResultsViewModel context => new ResultsView { DataContext = context },

        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}