using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MatchRepository(LeagueDbContext dbContext) : IMatchRepository
{
    public async Task<Match?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Matches.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(Match match, CancellationToken cancellationToken = default)
        => await dbContext.Matches.AddAsync(match, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
