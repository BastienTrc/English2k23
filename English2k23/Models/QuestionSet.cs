using Avalonia.Collections;
using Avalonia.Data;
using ReactiveUI;

namespace English2k23.Models;

public class QuestionSet : ReactiveObject
{
    private string? _description;
    private string? _name;
    private string? _pictureUrl;
    private readonly AvaloniaList<Question?> questionList = new();

    public QuestionSet(string? name, string? description, string? pictureUrl)
    {
        _name = name;
        _description = description;
        PictureUrl = pictureUrl;
    }

    public string? Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DataValidationException("Description field can't be empty");
            this.RaiseAndSetIfChanged(ref _description, value);
        }
    }

    public string? Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new DataValidationException("Name field can't be empty");
            this.RaiseAndSetIfChanged(ref _name, value);
        }
    }

    public string? PictureUrl
    {
        get => _pictureUrl;
        set => this.RaiseAndSetIfChanged(ref _pictureUrl, value);
    }


    public AvaloniaList<Question?> getQuestions()
    {
        return questionList;
    }
}