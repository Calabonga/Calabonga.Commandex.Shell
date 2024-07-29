namespace Calabonga.Commandex.QuizActions;

public class Question
{
    public Guid Id { get; set; }

    public required string QuestionText { get; set; }

    public required string AnswerA { get; set; }

    public required string AnswerB { get; set; }

    public required string AnswerC { get; set; }

    public required string AnswerD { get; set; }

    public DateTime CreatedAt { get; set; }

    public int Level { get; set; }

    public required string CategoryName { get; set; }

    public Guid CategoryId { get; set; }

    public int CorrectAnswer { get; set; }
}