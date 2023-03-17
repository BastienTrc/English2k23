using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class CompetitiveViewModel : ReactiveObject, IRoutableViewModel
{
    private Game game;
    private QuestionStack stack;

    public CompetitiveViewModel(IScreen hostScreen, Game game, QuestionStack stack)
    {
        this.game = game;
        this.stack = stack;
        HostScreen = hostScreen;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }
}