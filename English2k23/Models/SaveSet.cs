using Avalonia.Collections;
using Avalonia.Data;
using ReactiveUI;
using System.Text.Json.Serialization;

namespace English2k23.Models;

public class SaveSet
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
    public SaveSet(){}
    public SaveSet(QuestionSet set)
    {
        Name = set.Name;
        Description = set.Description;
        PictureUrl = set.PictureUrl;
        questionList = new List<Question>();
        var ql = set.getQuestions();
        for (var i = 0; i < ql.Count; i++)
        {
            questionList.Add(ql[i]);
        }
    }
}