using Application.DTOs;
using Application.Exceptions;

namespace Api.Validators;

public static class DomainValidators
{
    public static void ValidateCreateTeam(CreateTeamRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["name"] = ["Team name is required."]
            });
        }
    }

    public static void ValidateUpdateTeam(UpdateTeamRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["name"] = ["Team name is required."]
            });
        }
    }

    public static void ValidateCreateMatchday(CreateMatchdayRequest request)
    {
        if (request.Number <= 0)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["number"] = ["Matchday number must be greater than zero."]
            });
        }
    }

    public static void ValidateCreateMatch(CreateMatchRequest request)
    {
        if (request.HomeTeamId == request.AwayTeamId)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["homeTeamId"] = ["Home and away teams must be different."],
                ["awayTeamId"] = ["Home and away teams must be different."]
            });
        }
    }

    public static void ValidateUpdateResult(UpdateResultRequest request)
    {
        if (request.HomeGoals < 0 || request.AwayGoals < 0)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["homeGoals"] = request.HomeGoals < 0 ? ["Goals cannot be negative."] : [],
                ["awayGoals"] = request.AwayGoals < 0 ? ["Goals cannot be negative."] : []
            }.Where(x => x.Value.Length > 0).ToDictionary(x => x.Key, x => x.Value));
        }
    }

    public static void ValidateGetStandings(int matchdayNumber)
    {
        if (matchdayNumber <= 0)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["matchdayNumber"] = ["Matchday number must be greater than zero."]
            });
        }
    }
}
