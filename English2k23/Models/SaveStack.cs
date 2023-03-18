using Avalonia.Collections;
using Avalonia.Data;
using ReactiveUI;
using System.Text.Json.Serialization;

namespace English2k23.Models;

public class SaveStack
{
    [JsonInclude]
    public string? Description { get; set; }
    [JsonInclude]
    public string? Name { get; set; }
    [JsonInclude]
    public string? PictureUrl { get; set; }
    [JsonInclude]
    public List<Question?> questionList { get; set; }
    
    [JsonConstructor]
    public SaveStack(){}
    public SaveStack(QuestionStack stack)
    {
        Name = stack.Name;
        Description = stack.Description;
        PictureUrl = stack.PictureUrl;
        questionList = new List<Question>();
        var ql = stack.getQuestions();
        for (var i = 0; i < ql.Count; i++)
        {
            questionList.Add(ql[i]);
        }
    }
}