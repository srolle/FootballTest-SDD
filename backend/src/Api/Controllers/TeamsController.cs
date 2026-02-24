using Api.Validators;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("teams")]
public class TeamsController(ITeamRepository teamRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Team>> CreateTeam([FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
    {
        DomainValidators.ValidateCreateTeam(request);
        var normalizedName = request.Name.Trim();

        if (await teamRepository.ExistsByNameAsync(normalizedName, cancellationToken))
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["name"] = ["Team name must be unique."]
            });
        }

        var team = new Team
        {
            Name = normalizedName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await teamRepository.AddAsync(team, cancellationToken);
        await teamRepository.SaveChangesAsync(cancellationToken);

        return Created($"/teams/{team.Id}", team);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Team>>> GetTeams(CancellationToken cancellationToken)
    {
        var teams = await teamRepository.GetAllAsync(cancellationToken);
        return Ok(teams);
    }

    [HttpPut("{teamId:guid}")]
    public async Task<ActionResult<Team>> UpdateTeam(Guid teamId, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        DomainValidators.ValidateUpdateTeam(request);

        var existingTeam = await teamRepository.GetByIdAsync(teamId, cancellationToken);
        if (existingTeam is null)
        {
            return NotFound();
        }

        var normalizedName = request.Name.Trim();
        if (!string.Equals(existingTeam.Name, normalizedName, StringComparison.OrdinalIgnoreCase)
            && await teamRepository.ExistsByNameAsync(normalizedName, cancellationToken))
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["name"] = ["Team name must be unique."]
            });
        }

        existingTeam.Name = normalizedName;
        await teamRepository.SaveChangesAsync(cancellationToken);

        return Ok(existingTeam);
    }

    [HttpPatch("{teamId:guid}/deactivate")]
    public async Task<ActionResult<Team>> DeactivateTeam(Guid teamId, CancellationToken cancellationToken)
    {
        var existingTeam = await teamRepository.GetByIdAsync(teamId, cancellationToken);
        if (existingTeam is null)
        {
            return NotFound();
        }

        existingTeam.IsActive = false;
        await teamRepository.SaveChangesAsync(cancellationToken);

        return Ok(existingTeam);
    }
}
