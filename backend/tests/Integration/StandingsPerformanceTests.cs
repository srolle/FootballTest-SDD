using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public class StandingsPerformanceTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public StandingsPerformanceTests(WebApplicationFactory<Program> baseFactory)
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
    public async Task GetStandings_ShouldMeetP90UnderTwoSeconds()
    {
        var client = factory.CreateClient();

        var teams = new List<Team>();
        for (var index = 1; index <= 6; index++)
        {
            var team = await (await client.PostAsJsonAsync("/teams", new { name = $"Team {index}" }))
                .Content
                .ReadFromJsonAsync<Team>();
            teams.Add(team!);
        }

        var matchday = await (await client.PostAsJsonAsync("/matchdays", new { number = 1 }))
            .Content
            .ReadFromJsonAsync<Matchday>();

        var fixtures = new[]
        {
            (Home: teams[0].Id, Away: teams[1].Id, HomeGoals: 2, AwayGoals: 1),
            (Home: teams[2].Id, Away: teams[3].Id, HomeGoals: 1, AwayGoals: 1),
            (Home: teams[4].Id, Away: teams[5].Id, HomeGoals: 0, AwayGoals: 3),
            (Home: teams[0].Id, Away: teams[2].Id, HomeGoals: 2, AwayGoals: 0),
            (Home: teams[3].Id, Away: teams[5].Id, HomeGoals: 1, AwayGoals: 2),
            (Home: teams[1].Id, Away: teams[4].Id, HomeGoals: 0, AwayGoals: 0)
        };

        foreach (var fixture in fixtures)
        {
            var match = await (await client.PostAsJsonAsync("/matches", new
            {
                matchdayId = matchday!.Id,
                homeTeamId = fixture.Home,
                awayTeamId = fixture.Away
            })).Content.ReadFromJsonAsync<Match>();

            var updateResult = await client.PutAsJsonAsync($"/matches/{match!.Id}/result", new
            {
                homeGoals = fixture.HomeGoals,
                awayGoals = fixture.AwayGoals
            });

            Assert.Equal(HttpStatusCode.OK, updateResult.StatusCode);
        }

        var durations = new List<long>();

        // Warm up request to avoid including first-time overhead in the sample.
        var warmUpResponse = await client.GetAsync("/standings?matchdayNumber=1");
        Assert.Equal(HttpStatusCode.OK, warmUpResponse.StatusCode);

        for (var index = 0; index < 30; index++)
        {
            var stopwatch = Stopwatch.StartNew();
            var response = await client.GetAsync("/standings?matchdayNumber=1");
            stopwatch.Stop();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            durations.Add(stopwatch.ElapsedMilliseconds);
        }

        durations.Sort();
        var percentileIndex = (int)Math.Ceiling(0.90 * durations.Count) - 1;
        var p90 = durations[Math.Clamp(percentileIndex, 0, durations.Count - 1)];

        Assert.True(p90 <= 2000, $"Expected p90 <= 2000ms but was {p90}ms.");
    }
}
