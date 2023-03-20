using System.Reactive;
using Avalonia.Collections;
using ReactiveUI;

namespace English2k23.Models;

public class Game
{
    private readonly Guid _id;

    public Game(Guid id, string name, string description)
    {
        Name = name;
        Description = description;
        _id = id;
        CardFrequency = CardFrequencyEnum.Middle;
    }

    public AvaloniaList<QuestionSet> QuestionStacks { get; } = new();
    public AvaloniaList<Question?> availableQuestions { get; } = new();

    private string Name { get; }
    private string Description { get; }
    private CardFrequencyEnum CardFrequency { get; }

    public string Id => _id.ToString();

    public void AddQuestionToGame(Question? question)
    {
        if (AlreadyHas(availableQuestions,question))
        {
            return;
        }
        availableQuestions.Add(question);
    }

    private bool AlreadyHas(AvaloniaList<Question?> questionsList, Question? newQuestion)
    {
        foreach (Question? question in questionsList)
        {
            if (question?.Expression == newQuestion?.Expression &&
                 question?.Definition == newQuestion?.Definition &&
                 question?.McqAnswers == newQuestion?.McqAnswers &&
                 question?.PathToVideo == newQuestion?.PathToVideo)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveQuestionFromGame(Question? question)
    {
        // Need to remove question from every stack it may be present
        foreach (var stack in QuestionStacks) RemoveQuestionFromStack(stack, question);

        availableQuestions.Remove(question);
    }

    public void AddQuestionToStack(QuestionSet questionSet, Question? question)
    {
        questionSet.getQuestions().Add(question);
    }

    public void RemoveQuestionFromStack(QuestionSet questionSet, Question? question)
    {
        questionSet.getQuestions().Remove(question);
    }

    public QuestionSet AddStack(QuestionSet questionSet)
    {
        QuestionStacks.Add(questionSet);
        return questionSet;
    }

    public ReactiveCommand<QuestionSet, Unit>? RemoveStack(QuestionSet questionSet)
    {
        QuestionStacks.Remove(questionSet);
        return null;
    }

    private enum CardFrequencyEnum
    {
        Low,
        Middle,
        High
    }
}