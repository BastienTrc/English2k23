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
        _id = Guid.NewGuid();
        CardFrequency = CardFrequencyEnum.Middle;
    }

    public AvaloniaList<QuestionStack> QuestionStacks { get; } = new();
    public AvaloniaList<Question?> availableQuestions { get; } = new();

    private string Name { get; }
    private string Description { get; }
    private CardFrequencyEnum CardFrequency { get; }

    public string Id => _id.ToString();

    public void AddQuestionToGame(Question? question)
    {
        availableQuestions.Add(question);
    }

    public void RemoveQuestionFromGame(Question? question)
    {
        // Need to remove question from every stack it may be present
        foreach (var stack in QuestionStacks) RemoveQuestionFromStack(stack, question);

        availableQuestions.Remove(question);
    }

    public void AddQuestionToStack(QuestionStack questionStack, Question? question)
    {
        questionStack.getQuestions().Add(question);
    }

    public void RemoveQuestionFromStack(QuestionStack questionStack, Question? question)
    {
        questionStack.getQuestions().Remove(question);
    }

    public QuestionStack AddStack(QuestionStack questionStack)
    {
        QuestionStacks.Add(questionStack);
        return questionStack;
    }

    public ReactiveCommand<QuestionStack, Unit>? RemoveStack(QuestionStack questionStack)
    {
        QuestionStacks.Remove(questionStack);
        return null;
    }

    private enum CardFrequencyEnum
    {
        Low,
        Middle,
        High
    }
}