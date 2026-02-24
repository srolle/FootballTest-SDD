using Domain.Entities;

namespace Application.Interfaces;

public interface IMatchdayRepository
{
    Task<Matchday?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(int number, CancellationToken cancellationToken = default);
    Task AddAsync(Matchday matchday, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
