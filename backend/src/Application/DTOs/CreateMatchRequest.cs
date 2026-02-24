namespace Application.DTOs;

public class CreateMatchRequest
{
    public Guid MatchdayId { get; set; }
    public Guid HomeTeamId { get; set; }
    public Guid AwayTeamId { get; set; }
}
