namespace Domain.Entities;

public class Matchday
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Number { get; set; }
    public string Status { get; set; } = "Scheduled";
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
