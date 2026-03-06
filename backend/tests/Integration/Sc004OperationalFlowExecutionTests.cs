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

public class Sc004OperationalFlowExecutionTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public Sc004OperationalFlowExecutionTests(WebApplicationFactory<Program> baseFactory)
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
    public async Task TenOperationalFlows_ShouldSucceedOnFirstAttempt()
    {
        var client = factory.CreateClient();
        var successfulAttempts = 0;
        var durationsMs = new List<long>();

        for (var index = 1; index <= 10; index++)
        {
            var stopwatch = Stopwatch.StartNew();
            var teamAResponse = await client.PostAsJsonAsync("/teams", new { name = $"SC004 Team A {index}" });
            var teamBResponse = await client.PostAsJsonAsync("/teams", new { name = $"SC004 Team B {index}" });
            var matchdayResponse = await client.PostAsJsonAsync("/matchdays", new { number = 200 + index });

            if (teamAResponse.StatusCode != HttpStatusCode.Created
                || teamBResponse.StatusCode != HttpStatusCode.Created
                || matchdayResponse.StatusCode != HttpStatusCode.Created)
            {
                continue;
            }

            var teamA = await teamAResponse.Content.ReadFromJsonAsync<Team>();
            var teamB = await teamBResponse.Content.ReadFromJsonAsync<Team>();
            var matchday = await matchdayResponse.Content.ReadFromJsonAsync<Matchday>();

            var matchResponse = await client.PostAsJsonAsync("/matches", new
            {
                matchdayId = matchday!.Id,
                homeTeamId = teamA!.Id,
                awayTeamId = teamB!.Id
            });

            if (matchResponse.StatusCode != HttpStatusCode.Created)
            {
                continue;
            }

            var match = await matchResponse.Content.ReadFromJsonAsync<Match>();

            var resultResponse = await client.PutAsJsonAsync($"/matches/{match!.Id}/result", new
            {
                homeGoals = 2,
                awayGoals = 1
            });

            var standingsResponse = await client.GetAsync($"/standings?matchdayNumber={200 + index}");

            if (resultResponse.StatusCode == HttpStatusCode.OK && standingsResponse.StatusCode == HttpStatusCode.OK)
            {
                successfulAttempts++;
            }

            stopwatch.Stop();
            durationsMs.Add(stopwatch.ElapsedMilliseconds);
        }

        var successRate = successfulAttempts * 100.0 / 10.0;
        var ordered = durationsMs.OrderBy(x => x).ToList();
        var avg = ordered.Average();
        var p95Index = (int)Math.Ceiling(0.95 * ordered.Count) - 1;
        var p95 = ordered[Math.Clamp(p95Index, 0, ordered.Count - 1)];
        var min = ordered.First();
        var max = ordered.Last();

        Console.WriteLine("SC004_SAMPLE=10");
        Console.WriteLine($"SC004_SUCCESS_RATE={successRate:F2}");
        Console.WriteLine($"SC004_AVG_FLOW_MS={avg:F2}");
        Console.WriteLine($"SC004_P95_FLOW_MS={p95}");
        Console.WriteLine($"SC004_MIN_FLOW_MS={min}");
        Console.WriteLine($"SC004_MAX_FLOW_MS={max}");

        Assert.True(successfulAttempts >= 10, $"Expected 10 successful first-attempt flows, got {successfulAttempts}.");
    }
}
