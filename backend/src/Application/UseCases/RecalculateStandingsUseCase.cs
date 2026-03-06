using Application.Interfaces;
using Domain.Entities;
using Domain.Services;

namespace Application.UseCases;

public class RecalculateStandingsUseCase(
    IMatchdayRepository matchdayRepository,
    IMatchRepository matchRepository,
    ITeamRepository teamRepository,
    IStandingEntryRepository standingEntryRepository,
    StandingsCalculator standingsCalculator)
{
    public async Task<IReadOnlyList<StandingEntry>> ExecuteAsync(Guid matchdayId, CancellationToken cancellationToken = default)
    {
        var matchday = await matchdayRepository.GetByIdAsync(matchdayId, cancellationToken);
        if (matchday is null)
        {
            throw new KeyNotFoundException("Matchday was not found.");
        }

        var teams = await teamRepository.GetAllAsync(cancellationToken);
        var finishedMatches = await matchRepository.GetFinishedUpToMatchdayNumberAsync(matchday.Number, cancellationToken);

        var standings = standingsCalculator.Calculate(matchday.Id, teams, finishedMatches).ToList();

        await standingEntryRepository.ReplaceForMatchdayAsync(matchday.Id, standings, cancellationToken);
        await standingEntryRepository.SaveChangesAsync(cancellationToken);

        return standings;
    }
}