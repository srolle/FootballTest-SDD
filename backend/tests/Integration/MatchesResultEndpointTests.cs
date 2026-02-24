using System.Net;
using System.Net.Http.Json;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Integration;

public class MatchesResultEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MatchesResultEndpointTests(WebApplicationFactory<Program> factory)
    {
        var dbPath = Path.Combine(Path.GetTempPath(), $"league-integration-{Guid.NewGuid():N}.db");
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.UseSetting("ConnectionStrings:LeagueDb", $"Data Source={dbPath}");
        });

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LeagueDbContext>();
        dbContext.Database.Migrate();
    }

    [Fact]
    public async Task PutMatchResult_ShouldReturnOk_AndPersistFinishedResult()
    {
        var client = _factory.CreateClient();

        var createdTeamA = await (await client.PostAsJsonAsync("/teams", new { name = "Team A" })).Content.ReadFromJsonAsync<Team>();
        var createdTeamB = await (await client.PostAsJsonAsync("/teams", new { name = "Team B" })).Content.ReadFromJsonAsync<Team>();
        var createdMatchday = await (await client.PostAsJsonAsync("/matchdays", new { number = 1 })).Content.ReadFromJsonAsync<Matchday>();
        var createdMatch = await (await client.PostAsJsonAsync("/matches", new
        {
            matchdayId = createdMatchday!.Id,
            homeTeamId = createdTeamA!.Id,
            awayTeamId = createdTeamB!.Id
        })).Content.ReadFromJsonAsync<Match>();

        var response = await client.PutAsJsonAsync($"/matches/{createdMatch!.Id}/result", new
        {
            homeGoals = 3,
            awayGoals = 2
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<LeagueDbContext>();
        var persistedMatch = await dbContext.Matches.FirstAsync(x => x.Id == createdMatch.Id);
        var changeLog = await dbContext.ResultChangeLogs.FirstOrDefaultAsync(x => x.MatchId == createdMatch.Id);

        Assert.Equal(3, persistedMatch.HomeGoals);
        Assert.Equal(2, persistedMatch.AwayGoals);
        Assert.Equal("Finished", persistedMatch.Status);
        Assert.NotNull(changeLog);
        Assert.Equal("ResultRegistered", changeLog!.ChangeType);
    }
}
