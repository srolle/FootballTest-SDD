using Api.Validators;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("matchdays")]
public class MatchdaysController(IMatchdayRepository matchdayRepository) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Matchday>> CreateMatchday([FromBody] CreateMatchdayRequest request, CancellationToken cancellationToken)
    {
        DomainValidators.ValidateCreateMatchday(request);

        if (await matchdayRepository.ExistsByNumberAsync(request.Number, cancellationToken))
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["number"] = ["Matchday number already exists."]
            });
        }

        var matchday = new Matchday
        {
            Number = request.Number,
            Status = "Scheduled"
        };

        await matchdayRepository.AddAsync(matchday, cancellationToken);
        await matchdayRepository.SaveChangesAsync(cancellationToken);

        return Created($"/matchdays/{matchday.Id}", matchday);
    }
}
