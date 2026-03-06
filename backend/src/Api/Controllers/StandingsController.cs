using Api.Validators;
using Application.DTOs;
using Application.Interfaces;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("standings")]
public class StandingsController(
    GetStandingsByMatchdayUseCase getStandingsByMatchdayUseCase,
    ITeamRepository teamRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<StandingEntryResponse>>> GetStandings(
        [FromQuery] int matchdayNumber,
        CancellationToken cancellationToken)
    {
        DomainValidators.ValidateGetStandings(matchdayNumber);

        var standings = await getStandingsByMatchdayUseCase.ExecuteAsync(matchdayNumber, cancellationToken);
        var teams = await teamRepository.GetAllAsync(cancellationToken);
        var teamNamesById = teams.ToDictionary(x => x.Id, x => x.Name);

        var response = standings.Select(x => new StandingEntryResponse
        {
            TeamId = x.TeamId,
            TeamName = teamNamesById.TryGetValue(x.TeamId, out var teamName) ? teamName : "Unknown Team",
            Played = x.Played,
            Won = x.Won,
            Drawn = x.Drawn,
            Lost = x.Lost,
            GoalsFor = x.GoalsFor,
            GoalsAgainst = x.GoalsAgainst,
            GoalDifference = x.GoalDifference,
            Points = x.Points,
            Position = x.Position
        }).ToList();

        return Ok(response);
    }
}