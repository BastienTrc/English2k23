using System.Reactive;
using Avalonia.Collections;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class AddExistingQuestionViewModel : ViewModelBase
{
    public AddExistingQuestionViewModel(Game game)
    {
        ShowDialog = new Interaction<Unit, string?>();
        QuestionList = game.availableQuestions;

        Validate = ReactiveCommand.Create(() =>
        {
            if (SelectedItems.Count == 0) return null;
            return SelectedItems;
        });
    }

    public ReactiveCommand<Unit, AvaloniaList<Question>?> Validate { get; }
    public AvaloniaList<Question?> QuestionList { get; }
    public AvaloniaList<Question> SelectedItems { get; } = new();

    public Interaction<Unit, string?> ShowDialog { get; }
}