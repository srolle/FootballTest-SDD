using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TeamRepository(LeagueDbContext dbContext) : ITeamRepository
{
    public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Teams.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Team>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.Teams.AsNoTracking().OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        => await dbContext.Teams.AnyAsync(x => x.Name == name, cancellationToken);

    public async Task AddAsync(Team team, CancellationToken cancellationToken = default)
        => await dbContext.Teams.AddAsync(team, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
