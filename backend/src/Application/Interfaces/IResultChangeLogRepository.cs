using Domain.Entities;

namespace Application.Interfaces;

public interface IResultChangeLogRepository
{
    Task AddAsync(ResultChangeLog log, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
