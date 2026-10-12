using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Infrastructure.Persistence;

namespace PS.TaskTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
