using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;

namespace Application.UseCases;

public class UpdateMatchResultUseCase(
    IMatchRepository matchRepository,
    IResultChangeLogRepository resultChangeLogRepository,
    RecalculateStandingsUseCase recalculateStandingsUseCase)
{
    public async Task<Match> ExecuteAsync(Guid matchId, UpdateResultRequest request, CancellationToken cancellationToken = default)
    {
        if (request.HomeGoals < 0 || request.AwayGoals < 0)
        {
            throw new RequestValidationException(new Dictionary<string, string[]>
            {
                ["homeGoals"] = request.HomeGoals < 0 ? ["Goals cannot be negative."] : [],
                ["awayGoals"] = request.AwayGoals < 0 ? ["Goals cannot be negative."] : []
            }.Where(x => x.Value.Length > 0).ToDictionary(x => x.Key, x => x.Value));
        }

        var match = await matchRepository.GetByIdAsync(matchId, cancellationToken);
        if (match is null)
        {
            throw new KeyNotFoundException("Match was not found.");
        }

        var changeType = match.Status == "Finished" ? "ResultUpdated" : "ResultRegistered";

        match.HomeGoals = request.HomeGoals;
        match.AwayGoals = request.AwayGoals;
        match.Status = "Finished";
        match.UpdatedAt = DateTime.UtcNow;

        await matchRepository.SaveChangesAsync(cancellationToken);

        var changeLog = new ResultChangeLog
        {
            MatchId = match.Id,
            ChangeType = changeType,
            ChangedAt = DateTime.UtcNow
        };

        await resultChangeLogRepository.AddAsync(changeLog, cancellationToken);
        await resultChangeLogRepository.SaveChangesAsync(cancellationToken);

        await recalculateStandingsUseCase.ExecuteAsync(match.MatchdayId, cancellationToken);

        return match;
    }
}
