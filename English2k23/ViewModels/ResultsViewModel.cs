using System.Reactive;
using Avalonia.Collections;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class ResultsViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }

    private QuestionStack QuestionStack { get; }
    private int Score { get; }
    public List<Tuple<string, int>> Detail { get; }

    private ReactiveCommand<Unit, IRoutableViewModel?> GoTryAgain { get; }
    private ReactiveCommand<Unit, IRoutableViewModel?> GoToPractice { get; }

    public ResultsViewModel(IScreen hostScreen, QuestionStack questionStack, int score, List<int> scoreDetails)
    {
        HostScreen = hostScreen;
        QuestionStack = questionStack;
        Score = score;
        var questions = questionStack.getQuestions();

        Detail = new List<Tuple<string, int>>();
        for (var i = 0; i < scoreDetails.Count; i++)
        {
            Detail.Add(Tuple.Create<string, int>(questions[i]?.Expression ?? "ERROR", scoreDetails[i]));
        }

        GoToPractice = ReactiveCommand.CreateFromObservable(() =>
        {
            hostScreen.Router.NavigateBack.Execute();
            return hostScreen.Router.NavigateBack.Execute();
        });

    }
}