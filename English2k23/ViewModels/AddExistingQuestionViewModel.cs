using System.Reactive;
using Avalonia.Collections;
using English2k23.Models;
using ReactiveUI;

namespace English2k23.ViewModels;

public class AddExistingQuestionViewModel : ViewModelBase
{
    
    public ReactiveCommand<Unit, AvaloniaList<Question>?> Validate { get; }
    public AvaloniaList<Question?> QuestionList { get; }
    public AvaloniaList<Question> SelectedItems { get; } = new AvaloniaList<Question>();

    public AddExistingQuestionViewModel(Game game)
    {
        ShowDialog = new Interaction<Unit, string?>();
        QuestionList = game.availableQuestions;
        for (int i = 0; i < 15; i++)
        {
            QuestionList.Add(new Question("apr" + i, "def", "1 ; 2 ; 3", "path", false));
        }
        
        Validate = ReactiveCommand.Create(() =>
        {
            if (SelectedItems.Count == 0)
            {
                return null;
            }
            return SelectedItems;
        });
    }
    
    public Interaction<Unit, string?> ShowDialog { get; }
}