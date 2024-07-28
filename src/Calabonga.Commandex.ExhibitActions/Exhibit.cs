using System;

namespace Calabonga.Commandex.ExhibitActions;

public class Exhibit
{
    public DateTime CreatedAt { get; set; }
    public string HallName { get; set; }
    public Tag[] Tags { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsAdult { get; set; }
    public string Id { get; set; }
    public int Code { get; set; }
    public string Content { get; set; }
    public string HallId { get; set; }
    public string Title { get; set; }
}