using Application.DTOs;

namespace Api.Validators;

public static class DomainValidators
{
    public static void ValidateCreateTeam(CreateTeamRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Team name is required.");
        }
    }

    public static void ValidateCreateMatchday(CreateMatchdayRequest request)
    {
        if (request.Number <= 0)
        {
            throw new ArgumentException("Matchday number must be greater than zero.");
        }
    }

    public static void ValidateCreateMatch(CreateMatchRequest request)
    {
        if (request.HomeTeamId == request.AwayTeamId)
        {
            throw new ArgumentException("Home and away teams must be different.");
        }
    }

    public static void ValidateUpdateResult(UpdateResultRequest request)
    {
        if (request.HomeGoals < 0 || request.AwayGoals < 0)
        {
            throw new ArgumentException("Goals cannot be negative.");
        }
    }
}
