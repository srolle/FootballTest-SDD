using Domain.Entities;
using Domain.Services;

namespace Unit;

public class StandingsCalculationTests
{
    private readonly StandingsCalculator standingsCalculator = new();

    [Fact]
    public void Calculate_ShouldAssignThreePointsToWinningTeam()
    {
        var matchdayId = Guid.NewGuid();
        var homeTeam = new Team { Id = Guid.NewGuid(), Name = "Barcelona" };
        var awayTeam = new Team { Id = Guid.NewGuid(), Name = "Real Madrid" };

        var matches = new[]
        {
            new Match
            {
                MatchdayId = matchdayId,
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                HomeGoals = 2,
                AwayGoals = 1,
                Status = "Finished"
            }
        };

        var standings = standingsCalculator.Calculate(matchdayId, new[] { homeTeam, awayTeam }, matches);

        var homeEntry = standings.Single(x => x.TeamId == homeTeam.Id);
        var awayEntry = standings.Single(x => x.TeamId == awayTeam.Id);

        Assert.Equal(3, homeEntry.Points);
        Assert.Equal(1, homeEntry.Won);
        Assert.Equal(0, homeEntry.Drawn);
        Assert.Equal(0, homeEntry.Lost);
        Assert.Equal(2, homeEntry.GoalsFor);
        Assert.Equal(1, homeEntry.GoalsAgainst);
        Assert.Equal(1, homeEntry.GoalDifference);

        Assert.Equal(0, awayEntry.Points);
        Assert.Equal(0, awayEntry.Won);
        Assert.Equal(0, awayEntry.Drawn);
        Assert.Equal(1, awayEntry.Lost);
    }

    [Fact]
    public void Calculate_ShouldAssignOnePointEachOnDraw()
    {
        var matchdayId = Guid.NewGuid();
        var homeTeam = new Team { Id = Guid.NewGuid(), Name = "Team A" };
        var awayTeam = new Team { Id = Guid.NewGuid(), Name = "Team B" };

        var matches = new[]
        {
            new Match
            {
                MatchdayId = matchdayId,
                HomeTeamId = homeTeam.Id,
                AwayTeamId = awayTeam.Id,
                HomeGoals = 1,
                AwayGoals = 1,
                Status = "Finished"
            }
        };

        var standings = standingsCalculator.Calculate(matchdayId, new[] { homeTeam, awayTeam }, matches);

        var homeEntry = standings.Single(x => x.TeamId == homeTeam.Id);
        var awayEntry = standings.Single(x => x.TeamId == awayTeam.Id);

        Assert.Equal(1, homeEntry.Points);
        Assert.Equal(1, awayEntry.Points);
        Assert.Equal(1, homeEntry.Drawn);
        Assert.Equal(1, awayEntry.Drawn);
        Assert.Equal(0, homeEntry.Lost);
        Assert.Equal(0, awayEntry.Lost);
    }

    [Fact]
    public void Calculate_ShouldOrderByPointsThenGoalDifferenceThenGoalsFor()
    {
        var matchdayId = Guid.NewGuid();
        var teamA = new Team { Id = Guid.NewGuid(), Name = "Team A" };
        var teamB = new Team { Id = Guid.NewGuid(), Name = "Team B" };
        var teamC = new Team { Id = Guid.NewGuid(), Name = "Team C" };

        var matches = new[]
        {
            new Match
            {
                MatchdayId = matchdayId,
                HomeTeamId = teamA.Id,
                AwayTeamId = teamB.Id,
                HomeGoals = 2,
                AwayGoals = 1,
                Status = "Finished"
            },
            new Match
            {
                MatchdayId = matchdayId,
                HomeTeamId = teamC.Id,
                AwayTeamId = teamB.Id,
                HomeGoals = 1,
                AwayGoals = 0,
                Status = "Finished"
            }
        };

        var standings = standingsCalculator.Calculate(matchdayId, new[] { teamA, teamB, teamC }, matches);

        Assert.Equal(teamA.Id, standings[0].TeamId);
        Assert.Equal(1, standings[0].Position);
        Assert.Equal(teamC.Id, standings[1].TeamId);
        Assert.Equal(2, standings[1].Position);
        Assert.Equal(teamB.Id, standings[2].TeamId);
        Assert.Equal(3, standings[2].Position);
    }
}
