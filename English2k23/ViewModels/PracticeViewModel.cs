using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class PracticeViewModel : ReactiveObject, IRoutableViewModel
{
    private string _a = "&";
    public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }

    public void OneButtonClicked()
    {
        _a = "plo";
    }

    public PracticeViewModel(IScreen hostScreen, Game game)
    {
        HostScreen = hostScreen;
    }
}