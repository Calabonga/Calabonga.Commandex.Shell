namespace Calabonga.Commandex.GetMicroservicesInfoCommand;

public class Microservice
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}