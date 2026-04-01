using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.DAL.Context;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Implementations;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TaskFlowDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TaskFlowDb")));

        services.AddScoped<IGenericRepository<ProjectEntity>, GenericRepository<ProjectEntity>>();
        services.AddScoped<IGenericRepository<TaskCommentEntity>, GenericRepository<TaskCommentEntity>>();
        services.AddScoped<IGenericRepository<TaskTagEntity>, GenericRepository<TaskTagEntity>>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();

        return services;
    }
}
