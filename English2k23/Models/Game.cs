using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Collections;
using ReactiveUI;

namespace English2k23.Models;

public class Game
{
    public AvaloniaList<QuestionStack> QuestionStacks { get; } = new();
    private AvaloniaList<Question> availableQuestions = new();

    private string Name { get; set; }
    private string Description { get; set; }
    private CardFrequencyEnum CardFrequency { get; set; }
    private Guid _id;

    enum CardFrequencyEnum
    {
        Low,
        Middle,
        High
    }

    public string Id => _id.ToString();

    public Game(Guid id, string name, string description)
    {
        Name = name;
        Description = description;
        _id = Guid.NewGuid();
        CardFrequency = CardFrequencyEnum.Middle;
    }

    public void AddQuestionToGame(Question question)
    {
        availableQuestions.Add(question);
    }

    public void RemoveQuestionFromGame(Question question)
    {
        // Need to remove question from every stack it may be present
        foreach (var stack in QuestionStacks)
        {
            RemoveQuestionFromStack(stack, question);
        }

        availableQuestions.Remove(question);
    }

    public void AddQuestionToStack(QuestionStack questionStack, Question question)
    {
        questionStack.getQuestions().Add(question);
    }

    private void RemoveQuestionFromStack(QuestionStack questionStack, Question question)
    {
        questionStack.getQuestions().Remove(question);
    }

    public QuestionStack AddStack(QuestionStack questionStack)
    {
        QuestionStacks.Add(questionStack);
        return questionStack;
    }

    public ReactiveCommand<QuestionStack, Unit> RemoveStack(QuestionStack questionStack)
    {
        QuestionStacks.Remove(questionStack);
        return null;
    }
}