using System.Reactive;
using Avalonia.Collections;
using English2k23.Models;
using ReactiveUI;
using Timer = System.Timers.Timer;

namespace English2k23.ViewModels;

public class PracticeViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }

    private readonly QuestionStack _stack;
    private int _i = 1;

    public string? McqChosed { get; set; }
    public string? UserAnswer { get; set; }

    public ReactiveCommand<Unit, Unit> DisplayMcq { get; }
    public ReactiveCommand<Unit, Unit> DisplayVideo { get; }
    public ReactiveCommand<Unit, Unit> Validate { get; }
    public ReactiveCommand<Unit, Unit> DisplayNone { get; }


    private Question? _currQuestion;

    public Question? CurrQuestion
    {
        get => _currQuestion;
        set
        {
            this.RaiseAndSetIfChanged(ref _currQuestion, value);
            AnswersList = new AvaloniaList<string>(_currQuestion?.McqAnswers?.Split(";") ?? new[] { "Error" })
            {
                _currQuestion?.Expression ?? "Error"
            };
            Shuffle(AnswersList);
        }
    }

    private AvaloniaList<string>? _answersList;

    public AvaloniaList<string>? AnswersList
    {
        get => _answersList;
        set => this.RaiseAndSetIfChanged(ref _answersList, value);
    }

    private bool _showUserAnswer;

    public bool ShowUserAnswer
    {
        get => _showUserAnswer;
        set => this.RaiseAndSetIfChanged(ref _showUserAnswer, value);
    }


    private bool _showMcq;

    public bool ShowMcq
    {
        get => _showMcq;
        set => this.RaiseAndSetIfChanged(ref _showMcq, value);
    }

    private bool _showVideo;

    public bool ShowVideo
    {
        get => _showVideo;
        set => this.RaiseAndSetIfChanged(ref _showVideo, value);
    }

    private bool _answerStyleChosen;

    public bool AnswerStyleChosen
    {
        get => _answerStyleChosen;
        set => this.RaiseAndSetIfChanged(ref _answerStyleChosen, value);
    }

    private int NbOfQuestion { get; }

    private double _progressBarValue;

    public double ProgressBarValue
    {
        get => _progressBarValue;
        set => this.RaiseAndSetIfChanged(ref _progressBarValue, value);
    }

    private int _score;

    public int Score
    {
        get => _score;
        set => this.RaiseAndSetIfChanged(ref _score, value);
    }

    private string _infoMsg = "Find the expression!";
    private bool _hasAnswered;

    public string InfoMsg
    {
        get => _infoMsg;
        set => this.RaiseAndSetIfChanged(ref _infoMsg, value);
    }

    private int _timerValue;

    public int TimerValue
    {
        get => _timerValue;
        set => this.RaiseAndSetIfChanged(ref _timerValue, value);
    }

    // Just a nod to HP
    static readonly Random Rnd = new Random();
    private readonly List<string> _houses = new List<string> { "Gryffindor", "Slytherin", "HufflePuff", "Ravenclaw" };

    private Timer? _timer;
    private const int TimeLimit = 1000; // Set timer delay, actual time will be: TimeLimit/RefreshTime
    private const int RefreshTime = 10; // Delay between 2 refresh of the timer bar
    private bool _tooLate; // If user took too much time to answer
    private List<int> _scoreDetails = new List<int>(); // Details of each point of each question

    private ReactiveCommand<Unit, IRoutableViewModel> GoToResults { get; } // End of the game

    public PracticeViewModel(IScreen hostScreen, QuestionStack stack)
    {
        HostScreen = hostScreen;

        _stack = stack;
        AvaloniaList<Question?> questionList = _stack.getQuestions();
        CurrQuestion = questionList[0];
        NbOfQuestion = questionList.Count;

        DisplayNone = ReactiveCommand.Create(() => // No help
        {
            AnswerStyleChosen = true;
            ShowUserAnswer = true;
        });
        DisplayMcq = ReactiveCommand.Create(() => // MCQ help
        {
            AnswerStyleChosen = true;
            ShowMcq = true;
        });
        DisplayVideo = ReactiveCommand.Create(() => // Video help
        {
            AnswerStyleChosen = true;
            ShowVideo = true;
            ShowUserAnswer = true;
        });

        Validate = ReactiveCommand.Create(() => // User confirm answer
        {
            _timer?.Stop(); // Stop the timer

            if (_hasAnswered) // Prevent user from clicking twice validate button
            {
                NextQuestion();
                return;
            }

            var currAnswer = _currQuestion?.Expression;
            var rndNumber = Rnd.Next(_houses.Count);

            if (_tooLate) // User took too many time to answer, no points will be earned
            {
                _tooLate = false;
                _scoreDetails.Add(0);
                NextQuestion();
                return;
            }

            if (ShowUserAnswer && string.Equals(UserAnswer, currAnswer,
                    StringComparison.CurrentCultureIgnoreCase))
            {
                Score += 5;
                _scoreDetails.Add(5);
                InfoMsg = $"Good answer, 5 points to {_houses[rndNumber]}!";
            }
            else if (ShowMcq && McqChosed == currAnswer)
            {
                Score += 3;
                _scoreDetails.Add(3);
                InfoMsg = $"Good answer, 3 points to {_houses[rndNumber]}!";
            }
            else if (ShowVideo && string.Equals(UserAnswer, currAnswer,
                         StringComparison.CurrentCultureIgnoreCase))
            {
                Score += 1;
                _scoreDetails.Add(1);
                InfoMsg = $"Good answer, 1 points to {_houses[rndNumber]}!";
            }
            else
            {
                _scoreDetails.Add(0);
                InfoMsg = $"Wrong answer, expected expression was {currAnswer}, no points to {_houses[rndNumber]}!";
            }

            _hasAnswered = true;
        });

        GoToResults = ReactiveCommand.CreateFromObservable(() =>
            HostScreen.Router.Navigate.Execute(new ResultsViewModel(HostScreen, stack, Score, _scoreDetails)));

        QuestionCountdown();
    }

    private void NextQuestion()
    {
        _hasAnswered = false;
        TimerValue = 0;
        InfoMsg = "Find the expression!";
        UserAnswer = "";

        if (_i >= NbOfQuestion)
        {
            // Open pop up end game
            InfoMsg = "End of the test!";
            GoToResults.Execute();
            return;
        }

        CurrQuestion = _stack.getQuestions()[_i++];
        ProgressBarValue += (100.0 / NbOfQuestion);

        ShowMcq = false;
        ShowUserAnswer = false;
        ShowVideo = false;
        AnswerStyleChosen = false;

        QuestionCountdown();
    }

    private void QuestionCountdown()
    {
        _timer = new Timer(RefreshTime);
        _timer.Elapsed += (_, _) =>
        {
            if (TimerValue == TimeLimit)
            {
                InfoMsg = $"Too late, no points can be earned! The answer was {_currQuestion?.Expression}";
                _tooLate = true;
                AnswerStyleChosen = true;
                return;
            }

            TimerValue += TimeLimit / (RefreshTime * 100);
        };

        _timer.AutoReset = true;
        _timer.Enabled = true;
        _timer.Start();
    }

    private static void Shuffle<T>(IList<T>? list)
    {
        var rng = new Random();
        if (list == null) return;
        var n = list.Count;

        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }

    private void Reset()
    {
        _i = 0;
        ProgressBarValue = 0;
        Score = 0;
        _scoreDetails = new List<int>();

    }
}