using Application.Interfaces;
using Application.UseCases;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("LeagueDb") ?? "Data Source=league.db";
        services.AddDbContext<LeagueDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IMatchdayRepository, MatchdayRepository>();
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<IResultChangeLogRepository, ResultChangeLogRepository>();
        services.AddScoped<UpdateMatchResultUseCase>();

        return services;
    }
}
