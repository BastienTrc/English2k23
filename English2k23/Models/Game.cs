using System;
using System.Collections.Generic;

namespace English2k23.Models;

public class Game
{
    private List<QuestionStack> questionStacks = new List<QuestionStack>();
    private List<Question> availableQuestions = new List<Question>();

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

    public void AddQuestionToGame(Question question) {
        availableQuestions.Add(question);
    }

    public void RemoveQuestionFromGame(Question question) {
        // Need to remove question from every stack it may be present
        questionStacks.ForEach(stack => RemoveQuestionFromStack(stack, question));

        availableQuestions.Remove(question);
    }

    public void AddQuestionToStack(QuestionStack questionStack, Question question) {
        questionStack.getQuestions().Add(question);
    }

    public void RemoveQuestionFromStack(QuestionStack questionStack, Question question) {
        questionStack.getQuestions().Remove(question);
    }

}