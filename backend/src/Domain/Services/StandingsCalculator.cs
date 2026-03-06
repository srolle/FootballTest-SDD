using Domain.Entities;

namespace Domain.Services;

public class StandingsCalculator
{
    public IReadOnlyList<StandingEntry> Calculate(
        Guid matchdayId,
        IEnumerable<Team> teams,
        IEnumerable<Match> matches)
    {
        ArgumentNullException.ThrowIfNull(teams);
        ArgumentNullException.ThrowIfNull(matches);

        var standingsByTeam = teams
            .GroupBy(x => x.Id)
            .ToDictionary(
                x => x.Key,
                x => new StandingEntry
                {
                    MatchdayId = matchdayId,
                    TeamId = x.Key
                });

        foreach (var match in matches.Where(IsFinishedWithScore))
        {
            var homeEntry = GetOrCreateEntry(standingsByTeam, matchdayId, match.HomeTeamId);
            var awayEntry = GetOrCreateEntry(standingsByTeam, matchdayId, match.AwayTeamId);

            ApplyMatchStatistics(homeEntry, awayEntry, match.HomeGoals!.Value, match.AwayGoals!.Value);
        }

        var ordered = standingsByTeam.Values
            .Select(x =>
            {
                x.GoalDifference = x.GoalsFor - x.GoalsAgainst;
                return x;
            })
            .OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.GoalDifference)
            .ThenByDescending(x => x.GoalsFor)
            .ThenBy(x => x.TeamId)
            .ToList();

        for (var index = 0; index < ordered.Count; index++)
        {
            ordered[index].Position = index + 1;
        }

        return ordered;
    }

    private static StandingEntry GetOrCreateEntry(
        IDictionary<Guid, StandingEntry> standingsByTeam,
        Guid matchdayId,
        Guid teamId)
    {
        if (standingsByTeam.TryGetValue(teamId, out var entry))
        {
            return entry;
        }

        entry = new StandingEntry
        {
            MatchdayId = matchdayId,
            TeamId = teamId
        };

        standingsByTeam[teamId] = entry;
        return entry;
    }

    private static bool IsFinishedWithScore(Match match)
        => string.Equals(match.Status, "Finished", StringComparison.OrdinalIgnoreCase)
        && match.HomeGoals.HasValue
        && match.AwayGoals.HasValue;

    private static void ApplyMatchStatistics(
        StandingEntry homeEntry,
        StandingEntry awayEntry,
        int homeGoals,
        int awayGoals)
    {
        homeEntry.Played += 1;
        awayEntry.Played += 1;

        homeEntry.GoalsFor += homeGoals;
        homeEntry.GoalsAgainst += awayGoals;

        awayEntry.GoalsFor += awayGoals;
        awayEntry.GoalsAgainst += homeGoals;

        if (homeGoals > awayGoals)
        {
            homeEntry.Won += 1;
            homeEntry.Points += 3;
            awayEntry.Lost += 1;
            return;
        }

        if (awayGoals > homeGoals)
        {
            awayEntry.Won += 1;
            awayEntry.Points += 3;
            homeEntry.Lost += 1;
            return;
        }

        homeEntry.Drawn += 1;
        awayEntry.Drawn += 1;
        homeEntry.Points += 1;
        awayEntry.Points += 1;
    }
}