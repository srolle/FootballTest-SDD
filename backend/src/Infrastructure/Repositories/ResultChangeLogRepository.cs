using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class ResultChangeLogRepository(LeagueDbContext dbContext) : IResultChangeLogRepository
{
    public async Task AddAsync(ResultChangeLog log, CancellationToken cancellationToken = default)
        => await dbContext.ResultChangeLogs.AddAsync(log, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
