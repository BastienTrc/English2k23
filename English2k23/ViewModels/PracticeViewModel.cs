using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Controls;
using English2k23.Models;
using LibVLCSharp.Shared;
using ReactiveUI;
using Timer = System.Timers.Timer;

namespace English2k23.ViewModels;

public class PracticeViewModel : ReactiveObject, IRoutableViewModel
{
    private const int TimeLimit = 10000; // Set timer delay
    private const int RefreshTime = 10; // Delay between 2 refresh of the timer bar

    // Just a nod to HP
    private static readonly Random Rnd = new();
    private readonly List<string> _houses = new() { "Gryffindor", "Slytherin", "HufflePuff", "Ravenclaw" };

    private readonly QuestionSet _set;

    private AvaloniaList<string>? _answersList;

    private bool _answerStyleChosen;


    private Question? _currQuestion;
    private bool _hasAnswered;
    private int _i = 1;

    private string _infoMsg = "Find the expression!";

    private readonly LibVLC? _libVlc;

    private double _progressBarValue;

    private int _score;
    private readonly List<int> _scoreDetails = new(); // Details of each point of each question


    private bool _showMcq;

    private bool _showUserAnswer;

    private bool _showVideo;
    
    
    public bool IsCompetitive { get; }

    private Timer? _timer;

    private int _timerValue;
    private bool _tooLate; // If user took too much time to answer
    public MediaPlayer? MediaPlayer;

    public PracticeViewModel(IScreen hostScreen, QuestionSet set, bool isCompetitive)
    {
        HostScreen = hostScreen;

        IsCompetitive = isCompetitive;
        
        _set = set;
        var questionList = _set.getQuestions();
        Shuffle(questionList);
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

        if (!Design.IsDesignMode)
        {
            {
                Core.Initialize();
            }

            _libVlc = new LibVLC(
                true
            );
            _libVlc.Log += VlcLogger_Event;

            MediaPlayer = new MediaPlayer(_libVlc);
            MediaPlayer.EndReached += MediaPlayer_EndReached;
            MediaPlayer.Fullscreen = true;
        }

        ShowVideoDialog = new Interaction<VideoPlayerModel, Unit>();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {

            DisplayVideo = ReactiveCommand.CreateFromTask(async () =>
            {
                var videoPlayerModel = new VideoPlayerModel(_currQuestion?.PathToVideo);

                AnswerStyleChosen = true;
                ShowUserAnswer = true;
                ShowVideo = true;

                _timer?.Stop();
                await ShowVideoDialog.Handle(videoPlayerModel);

                _timer?.Start();
            });
        }

        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            DisplayVideo = ReactiveCommand.CreateFromTask( () =>
                {
                    _timer?.Stop();
                AnswerStyleChosen = true;
                ShowUserAnswer = true;
                ShowVideo = true;

                Play();
                return null;
            });
        }


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

            if (ShowUserAnswer && !ShowVideo && string.Equals(UserAnswer, currAnswer,
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
            UserAnswer = "";
        });

        GoToResults = ReactiveCommand.CreateFromObservable(() =>
            HostScreen.Router.Navigate.Execute(new ResultsViewModel(HostScreen, set, Score, _scoreDetails)));

        QuestionCountdown();
    }

    public string? McqChosed { get; set; }
    public string? UserAnswer { get; set; }

    public ReactiveCommand<Unit, Unit> DisplayMcq { get; }
    public ReactiveCommand<Unit, Unit> Validate { get; }
    public ReactiveCommand<Unit, Unit> DisplayNone { get; }

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

    public AvaloniaList<string>? AnswersList
    {
        get => _answersList;
        set => this.RaiseAndSetIfChanged(ref _answersList, value);
    }

    public bool ShowUserAnswer
    {
        get => _showUserAnswer;
        set => this.RaiseAndSetIfChanged(ref _showUserAnswer, value);
    }

    public bool ShowMcq
    {
        get => _showMcq;
        set => this.RaiseAndSetIfChanged(ref _showMcq, value);
    }

    public bool ShowVideo
    {
        get => _showVideo;
        set => this.RaiseAndSetIfChanged(ref _showVideo, value);
    }

    public bool AnswerStyleChosen
    {
        get => _answerStyleChosen;
        set => this.RaiseAndSetIfChanged(ref _answerStyleChosen, value);
    }

    private int NbOfQuestion { get; }

    public double ProgressBarValue
    {
        get => _progressBarValue;
        set => this.RaiseAndSetIfChanged(ref _progressBarValue, value);
    }

    public int Score
    {
        get => _score;
        set => this.RaiseAndSetIfChanged(ref _score, value);
    }

    public string InfoMsg
    {
        get => _infoMsg;
        set => this.RaiseAndSetIfChanged(ref _infoMsg, value);
    }

    public int TimerValue
    {
        get => _timerValue;
        set => this.RaiseAndSetIfChanged(ref _timerValue, value);
    }

    private ReactiveCommand<Unit, IRoutableViewModel> GoToResults { get; } // End of the game

    // Handle add stack command
    public ICommand? DisplayVideo { get; }
    public Interaction<VideoPlayerModel, Unit> ShowVideoDialog { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    public IScreen HostScreen { get; }

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

        CurrQuestion = _set.getQuestions()[_i++];
        ProgressBarValue += 100.0 / NbOfQuestion;

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
            if (TimerValue == 1000 && IsCompetitive) // Max value of the progressBar
            {
                InfoMsg = $"Too late, no points can be earned! The answer was {_currQuestion?.Expression}";
                _tooLate = true;
                AnswerStyleChosen = true;
                return;
            }

            TimerValue += 1000*RefreshTime/TimeLimit;
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

    private void VlcLogger_Event(object? sender, LogEventArgs l)
    {
        Debug.WriteLine(l.Message);
    }

    public void Play()
    {
        if (_libVlc != null && MediaPlayer != null)
        {
            //string[] Media_AdditionalOptions = {
            //    $":avcodec-hw=any"
            //};
            string[] mediaAdditionalOptions = { };

            using var media = new Media(
                _libVlc,
                new Uri(
                    $"{AppDomain.CurrentDomain.BaseDirectory}Videos{Path.DirectorySeparatorChar}{CurrQuestion?.PathToVideo}"),
                mediaAdditionalOptions
            );
            MediaPlayer.Play(media);
            media.Dispose();
        }
    }

    public void Stop()
    {
        if (MediaPlayer != null) MediaPlayer.Stop();
    }

    private void MediaPlayer_EndReached(object? sender, EventArgs e)
    {
        ThreadPool.QueueUserWorkItem(_ => MediaPlayer?.Stop());
        _timer?.Start();
    }
}
