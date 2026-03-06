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

public class Sc001ResultRegistrationExecutionTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public Sc001ResultRegistrationExecutionTests(WebApplicationFactory<Program> baseFactory)
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
    public async Task RegisteringFiftyResults_ShouldMeetSc001Threshold()
    {
        var client = factory.CreateClient();

        var homeTeam = await (await client.PostAsJsonAsync("/teams", new { name = "SC001 Team Home" }))
            .Content
            .ReadFromJsonAsync<Team>();

        var awayTeam = await (await client.PostAsJsonAsync("/teams", new { name = "SC001 Team Away" }))
            .Content
            .ReadFromJsonAsync<Team>();

        var matchday = await (await client.PostAsJsonAsync("/matchdays", new { number = 101 }))
            .Content
            .ReadFromJsonAsync<Matchday>();

        var durationsMs = new List<long>();

        for (var index = 0; index < 50; index++)
        {
            var match = await (await client.PostAsJsonAsync("/matches", new
            {
                matchdayId = matchday!.Id,
                homeTeamId = homeTeam!.Id,
                awayTeamId = awayTeam!.Id
            })).Content.ReadFromJsonAsync<Match>();

            var stopwatch = Stopwatch.StartNew();
            var updateResponse = await client.PutAsJsonAsync($"/matches/{match!.Id}/result", new
            {
                homeGoals = (index % 4) + 1,
                awayGoals = index % 3
            });
            stopwatch.Stop();

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
            durationsMs.Add(stopwatch.ElapsedMilliseconds);
        }

        var successfulWithinThreshold = durationsMs.Count(x => x <= 60_000);
        var successRate = successfulWithinThreshold * 100.0 / durationsMs.Count;

        var ordered = durationsMs.OrderBy(x => x).ToList();
        var avg = ordered.Average();
        var p95Index = (int)Math.Ceiling(0.95 * ordered.Count) - 1;
        var p95 = ordered[Math.Clamp(p95Index, 0, ordered.Count - 1)];
        var min = ordered.First();
        var max = ordered.Last();

        Console.WriteLine($"SC001_SAMPLE={ordered.Count}");
        Console.WriteLine($"SC001_SUCCESS_RATE={successRate:F2}");
        Console.WriteLine($"SC001_AVG_MS={avg:F2}");
        Console.WriteLine($"SC001_P95_MS={p95}");
        Console.WriteLine($"SC001_MIN_MS={min}");
        Console.WriteLine($"SC001_MAX_MS={max}");

        Assert.True(successRate >= 95, $"Expected >=95% requests under 60s, got {successRate:F2}%.");
    }
}
