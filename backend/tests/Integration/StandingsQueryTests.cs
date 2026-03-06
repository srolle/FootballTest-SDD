using System.Net;
using System.Net.Http.Json;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public class StandingsQueryTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public StandingsQueryTests(WebApplicationFactory<Program> baseFactory)
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
    public async Task GetStandings_ByMatchdayNumber_ShouldReturnOrderedStandings()
    {
        var client = factory.CreateClient();

        var teamA = await (await client.PostAsJsonAsync("/teams", new { name = "Team A" })).Content.ReadFromJsonAsync<Team>();
        var teamB = await (await client.PostAsJsonAsync("/teams", new { name = "Team B" })).Content.ReadFromJsonAsync<Team>();
        var teamC = await (await client.PostAsJsonAsync("/teams", new { name = "Team C" })).Content.ReadFromJsonAsync<Team>();
        var matchday = await (await client.PostAsJsonAsync("/matchdays", new { number = 1 })).Content.ReadFromJsonAsync<Matchday>();

        var matchAB = await (await client.PostAsJsonAsync("/matches", new
        {
            matchdayId = matchday!.Id,
            homeTeamId = teamA!.Id,
            awayTeamId = teamB!.Id
        })).Content.ReadFromJsonAsync<Match>();

        var matchCB = await (await client.PostAsJsonAsync("/matches", new
        {
            matchdayId = matchday.Id,
            homeTeamId = teamC!.Id,
            awayTeamId = teamB!.Id
        })).Content.ReadFromJsonAsync<Match>();

        var updateResultA = await client.PutAsJsonAsync($"/matches/{matchAB!.Id}/result", new
        {
            homeGoals = 1,
            awayGoals = 0
        });
        Assert.Equal(HttpStatusCode.OK, updateResultA.StatusCode);

        var updateResultC = await client.PutAsJsonAsync($"/matches/{matchCB!.Id}/result", new
        {
            homeGoals = 2,
            awayGoals = 0
        });
        Assert.Equal(HttpStatusCode.OK, updateResultC.StatusCode);

        var response = await client.GetAsync("/standings?matchdayNumber=1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var standings = await response.Content.ReadFromJsonAsync<List<StandingRowResponse>>();

        Assert.NotNull(standings);
        Assert.Equal(3, standings!.Count);

        Assert.Equal("Team C", standings[0].TeamName);
        Assert.Equal(3, standings[0].Points);
        Assert.Equal(2, standings[0].GoalDifference);
        Assert.Equal(1, standings[0].Position);

        Assert.Equal("Team A", standings[1].TeamName);
        Assert.Equal(3, standings[1].Points);
        Assert.Equal(1, standings[1].GoalDifference);
        Assert.Equal(2, standings[1].Position);

        Assert.Equal("Team B", standings[2].TeamName);
        Assert.Equal(0, standings[2].Points);
        Assert.Equal(3, standings[2].GoalsAgainst);
        Assert.Equal(3, standings[2].Position);
    }

    private class StandingRowResponse
    {
        public string TeamName { get; set; } = string.Empty;
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
    }
}
