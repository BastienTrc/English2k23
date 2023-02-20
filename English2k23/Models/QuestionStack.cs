using System;
using System.Collections.Generic;
using Avalonia.Collections;

namespace English2k23.Models;

public class QuestionStack
{
    private AvaloniaList<Question> questionList = new();
    private string? PictureURL { get; set; }
    private string _name;
    private string _description;

    public string Description
    {
        get => _description;
        set => _description = value ?? throw new ArgumentNullException($"Description can't be null or empty");
    }

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException($"Name can't be null or empty");
    }

    public QuestionStack(string name, string description, string? pictureUrl)
    {
        _name = name;
        _description = description;
        PictureURL = pictureUrl;
    }


    public AvaloniaList<Question> getQuestions()
    {
        return questionList;
    }

}