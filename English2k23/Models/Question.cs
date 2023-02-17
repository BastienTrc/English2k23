using System;

namespace English2k23.Models;

public class Question
{
    private bool VideoMode { get; set; }
    private string _expression;
    private string _definition;
    private string _MCQAnswers;
    private string _pathToVideo;


    public Question(string expression, string definition, string mcqAnswers, string pathToVideo, bool videoMode)
    {
        _expression = expression;
        _definition = definition;
        _MCQAnswers = mcqAnswers;
        _pathToVideo = pathToVideo;
        VideoMode = videoMode;
    }


    public string Expression
    {
        get => _expression;
        set => _expression = value ?? throw new ArgumentNullException($"Expression can't be null or empty");
    }

    public string Definition
    {
        get => _definition;
        set => _definition = value ?? throw new ArgumentNullException($"Definition can't be null or empty");
    }

    public string McqAnswers
    {
        get => _MCQAnswers;
        set => _MCQAnswers = value ?? throw new ArgumentNullException($"MCQ answers can't be null or empty");
    }

    public string PathToVideo
    {
        get => _pathToVideo;
        set
        {
            if (value == null)
            {
                _pathToVideo = "ERROR_EMPTY";
            }
            else
            {
                _pathToVideo = value;
            }
        }
    }
}