using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StandingEntryRepository(LeagueDbContext dbContext) : IStandingEntryRepository
{
    public async Task<IReadOnlyList<StandingEntry>> GetByMatchdayIdAsync(Guid matchdayId, CancellationToken cancellationToken = default)
        => await dbContext.StandingEntries
            .AsNoTracking()
            .Where(x => x.MatchdayId == matchdayId)
            .OrderBy(x => x.Position)
            .ThenBy(x => x.TeamId)
            .ToListAsync(cancellationToken);

    public async Task ReplaceForMatchdayAsync(Guid matchdayId, IReadOnlyCollection<StandingEntry> entries, CancellationToken cancellationToken = default)
    {
        var existingEntries = await dbContext.StandingEntries
            .Where(x => x.MatchdayId == matchdayId)
            .ToListAsync(cancellationToken);

        if (existingEntries.Count > 0)
        {
            dbContext.StandingEntries.RemoveRange(existingEntries);
        }

        if (entries.Count > 0)
        {
            await dbContext.StandingEntries.AddRangeAsync(entries, cancellationToken);
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}