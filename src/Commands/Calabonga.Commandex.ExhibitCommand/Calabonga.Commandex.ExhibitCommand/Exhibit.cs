namespace Calabonga.Commandex.ExhibitCommand;

public class Exhibit
{
    public DateTime CreatedAt { get; set; }

    public string HallName { get; set; } = null!;

    public Tag[] Tags { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsAdult { get; set; }

    public Guid Id { get; set; }

    public int Code { get; set; }

    public string Content { get; set; } = null!;

    public Guid HallId { get; set; }

    public string Title { get; set; } = null!;
}