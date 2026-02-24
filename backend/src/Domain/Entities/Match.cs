namespace Domain.Entities;

public class Match
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MatchdayId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public string Status { get; set; } = "Scheduled";
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Matchday? Matchday { get; set; }
}
