using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MatchRepository(LeagueDbContext dbContext) : IMatchRepository
{
    public async Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Matches.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Match>> GetFinishedUpToMatchdayNumberAsync(int matchdayNumber, CancellationToken cancellationToken = default)
        => await dbContext.Matches
            .AsNoTracking()
            .Include(x => x.Matchday)
            .Where(x => x.Status == "Finished"
                && x.HomeGoals.HasValue
                && x.AwayGoals.HasValue
                && x.Matchday != null
                && x.Matchday.Number <= matchdayNumber)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Match match, CancellationToken cancellationToken = default)
        => await dbContext.Matches.AddAsync(match, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
