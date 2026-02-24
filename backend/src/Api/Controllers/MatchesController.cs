using Api.Validators;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("matches")]
public class MatchesController(
    IMatchRepository matchRepository,
    IMatchdayRepository matchdayRepository,
    ITeamRepository teamRepository,
    UpdateMatchResultUseCase updateMatchResultUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Match>> CreateMatch([FromBody] CreateMatchRequest request, CancellationToken cancellationToken)
    {
        DomainValidators.ValidateCreateMatch(request);

        var matchday = await matchdayRepository.GetByIdAsync(request.MatchdayId, cancellationToken);
        if (matchday is null)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["matchdayId"] = ["Matchday does not exist."]
            });
        }

        var homeTeam = await teamRepository.GetByIdAsync(request.HomeTeamId, cancellationToken);
        if (homeTeam is null || !homeTeam.IsActive)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["homeTeamId"] = ["Home team must exist and be active."]
            });
        }

        var awayTeam = await teamRepository.GetByIdAsync(request.AwayTeamId, cancellationToken);
        if (awayTeam is null || !awayTeam.IsActive)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["awayTeamId"] = ["Away team must exist and be active."]
            });
        }

        var match = new Match
        {
            MatchdayId = request.MatchdayId,
            HomeTeamId = request.HomeTeamId,
            AwayTeamId = request.AwayTeamId,
            Status = "Scheduled",
            UpdatedAt = DateTime.UtcNow
        };

        await matchRepository.AddAsync(match, cancellationToken);
        await matchRepository.SaveChangesAsync(cancellationToken);

        return Created($"/matches/{match.Id}", match);
    }

    [HttpPut("{matchId:guid}/result")]
    public async Task<ActionResult<Match>> UpdateResult(Guid matchId, [FromBody] UpdateResultRequest request, CancellationToken cancellationToken)
    {
        var match = await updateMatchResultUseCase.ExecuteAsync(matchId, request, cancellationToken);
        return Ok(match);
    }
}
