using System.Net;
using System.Net.Http.Json;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public class StandingsRecalculationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public StandingsRecalculationTests(WebApplicationFactory<Program> baseFactory)
    {
        var dbPath = Path.Combine(Path.GetTempPath(), $"league-integration-{Guid.NewGuid():N}.db");
        factory = baseFactory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.UseSetting("ConnectionStrings:LeagueDb", $"Data Source={dbPath}");
        });

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LeagueDbContext>();
        dbContext.Database.Migrate();
    }

    [Fact]
    public async Task PutMatchResult_WhenResultIsEdited_ShouldRecalculateStandingEntriesForMatchday()
    {
        var client = factory.CreateClient();

        var teamA = await (await client.PostAsJsonAsync("/teams", new { name = "Team A" })).Content.ReadFromJsonAsync<Team>();
        var teamB = await (await client.PostAsJsonAsync("/teams", new { name = "Team B" })).Content.ReadFromJsonAsync<Team>();
        var matchday = await (await client.PostAsJsonAsync("/matchdays", new { number = 1 })).Content.ReadFromJsonAsync<Matchday>();
        var match = await (await client.PostAsJsonAsync("/matches", new
        {
            matchdayId = matchday!.Id,
            homeTeamId = teamA!.Id,
            awayTeamId = teamB!.Id
        })).Content.ReadFromJsonAsync<Match>();

        var firstUpdate = await client.PutAsJsonAsync($"/matches/{match!.Id}/result", new
        {
            homeGoals = 1,
            awayGoals = 0
        });

        Assert.Equal(HttpStatusCode.OK, firstUpdate.StatusCode);

        var secondUpdate = await client.PutAsJsonAsync($"/matches/{match.Id}/result", new
        {
            homeGoals = 0,
            awayGoals = 2
        });

        Assert.Equal(HttpStatusCode.OK, secondUpdate.StatusCode);

        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LeagueDbContext>();
        var standings = await dbContext.StandingEntries
            .Where(x => x.MatchdayId == matchday.Id)
            .OrderBy(x => x.Position)
            .ToListAsync();

        var teamAStanding = standings.Single(x => x.TeamId == teamA.Id);
        var teamBStanding = standings.Single(x => x.TeamId == teamB.Id);

        Assert.Equal(2, standings.Count);
        Assert.Equal(0, teamAStanding.Points);
        Assert.Equal(1, teamAStanding.Played);
        Assert.Equal(1, teamAStanding.Lost);
        Assert.Equal(2, teamAStanding.Position);

        Assert.Equal(3, teamBStanding.Points);
        Assert.Equal(1, teamBStanding.Played);
        Assert.Equal(1, teamBStanding.Won);
        Assert.Equal(1, teamBStanding.Position);
    }
}
