using System;
using System.Collections.Generic;

namespace English2k23.Models;

public class QuestionStack
{
    private List<Question> questionList = new List<Question>();
    private string PictureURL { get; set; }
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

    public QuestionStack(string name, string description)
    {
        _name = name;
        _description = description;
    }


    public List<Question> getQuestions()
    {
        return questionList;
    }
}