using System.Reactive;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class ResultsViewModel : ReactiveObject, IRoutableViewModel
{
    public ResultsViewModel(IScreen hostScreen, QuestionSet questionSet, int score, List<int> scoreDetails)
    {
        HostScreen = hostScreen;
        QuestionSet = questionSet;
        Score = score;
        var questions = questionSet.getQuestions();

        Detail = new List<Tuple<string, int>>();
        for (var i = 0; i < scoreDetails.Count; i++)
            Detail.Add(Tuple.Create(questions[i]?.Expression ?? "ERROR", scoreDetails[i]));

        GoToPractice = ReactiveCommand.CreateFromObservable(() =>
        {
            hostScreen.Router.NavigateBack.Execute();
            return hostScreen.Router.NavigateBack.Execute();
        });
    }

    private QuestionSet QuestionSet { get; }
    private int Score { get; }
    public List<Tuple<string, int>> Detail { get; }

    private ReactiveCommand<Unit, IRoutableViewModel?>? GoTryAgain { get; }
    private ReactiveCommand<Unit, IRoutableViewModel?> GoToPractice { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }
}