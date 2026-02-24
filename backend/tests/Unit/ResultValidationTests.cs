using Api.Validators;
using Application.DTOs;
using Application.Exceptions;

namespace Unit;

public class ResultValidationTests
{
    [Fact]
    public void ValidateUpdateResult_ShouldThrow_WhenHomeGoalsIsNegative()
    {
        var request = new UpdateResultRequest
        {
            HomeGoals = -1,
            AwayGoals = 0
        };

        var exception = Assert.Throws<RequestValidationException>(() => DomainValidators.ValidateUpdateResult(request));

        Assert.True(exception.Errors.ContainsKey("homeGoals"));
    }

    [Fact]
    public void ValidateUpdateResult_ShouldThrow_WhenAwayGoalsIsNegative()
    {
        var request = new UpdateResultRequest
        {
            HomeGoals = 1,
            AwayGoals = -2
        };

        var exception = Assert.Throws<RequestValidationException>(() => DomainValidators.ValidateUpdateResult(request));

        Assert.True(exception.Errors.ContainsKey("awayGoals"));
    }

    [Fact]
    public void ValidateUpdateResult_ShouldNotThrow_WhenGoalsAreValid()
    {
        var request = new UpdateResultRequest
        {
            HomeGoals = 2,
            AwayGoals = 1
        };

        DomainValidators.ValidateUpdateResult(request);
    }
}
