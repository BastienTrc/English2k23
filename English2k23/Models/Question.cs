using Avalonia.Data;
using ReactiveUI;

namespace English2k23.Models;

public class Question : ReactiveObject
{
    private string? _definition;
    private string? _expression;
    private string? _MCQAnswers;
    private string _pathToVideo;


    public Question(string? expression, string? definition, string? mcqAnswers, string pathToVideo, bool videoMode)
    {
        _expression = expression;
        _definition = definition;
        _MCQAnswers = mcqAnswers;
        _pathToVideo = pathToVideo;
        VideoMode = videoMode;
    }

    public bool VideoMode { get; set; }


    public string? Expression
    {
        get => _expression;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DataValidationException("Expression field can't be empty");
            this.RaiseAndSetIfChanged(ref _expression, value);
        }
    }

    public string? Definition
    {
        get => _definition;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DataValidationException("Definition field can't be empty");
            this.RaiseAndSetIfChanged(ref _definition, value);
        }
    }

    public string? McqAnswers
    {
        get => _MCQAnswers;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DataValidationException("MCQ answers field can't be empty");
            this.RaiseAndSetIfChanged(ref _MCQAnswers, value);
        }
    }

    public string PathToVideo
    {
        get => _pathToVideo;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                this.RaiseAndSetIfChanged(ref _pathToVideo, "No video was given");
                return;
            }

            this.RaiseAndSetIfChanged(ref _pathToVideo, value);
        }
    }
    
    
}