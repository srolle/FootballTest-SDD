using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MatchdayRepository(LeagueDbContext dbContext) : IMatchdayRepository
{
    public async Task<Matchday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Matchdays.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Matchday?> GetByNumberAsync(int number, CancellationToken cancellationToken = default)
        => await dbContext.Matchdays.FirstOrDefaultAsync(x => x.Number == number, cancellationToken);

    public async Task<bool> ExistsByNumberAsync(int number, CancellationToken cancellationToken = default)
        => await dbContext.Matchdays.AnyAsync(x => x.Number == number, cancellationToken);

    public async Task AddAsync(Matchday matchday, CancellationToken cancellationToken = default)
        => await dbContext.Matchdays.AddAsync(matchday, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
