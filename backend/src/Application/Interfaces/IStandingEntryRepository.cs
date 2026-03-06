using Domain.Entities;

namespace Application.Interfaces;

public interface IStandingEntryRepository
{
    Task<IReadOnlyList<StandingEntry>> GetByMatchdayIdAsync(Guid matchdayId, CancellationToken cancellationToken = default);
    Task ReplaceForMatchdayAsync(Guid matchdayId, IReadOnlyCollection<StandingEntry> entries, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
