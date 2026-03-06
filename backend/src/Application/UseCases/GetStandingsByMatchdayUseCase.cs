using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases;

public class GetStandingsByMatchdayUseCase(
    IMatchdayRepository matchdayRepository,
    IStandingEntryRepository standingEntryRepository,
    RecalculateStandingsUseCase recalculateStandingsUseCase)
{
    public async Task<IReadOnlyList<StandingEntry>> ExecuteAsync(int matchdayNumber, CancellationToken cancellationToken = default)
    {
        var matchday = await matchdayRepository.GetByNumberAsync(matchdayNumber, cancellationToken);
        if (matchday is null)
        {
            throw new KeyNotFoundException("Matchday was not found.");
        }

        var standings = await standingEntryRepository.GetByMatchdayIdAsync(matchday.Id, cancellationToken);
        if (standings.Count > 0)
        {
            return standings;
        }

        return await recalculateStandingsUseCase.ExecuteAsync(matchday.Id, cancellationToken);
    }
}
