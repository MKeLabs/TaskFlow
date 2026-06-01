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

        services.AddScoped<IGenericRepository<ProjectEntity>, ProjectsRepository>();
        services.AddScoped<IGenericRepository<TaskCommentEntity>, TaskCommentRepository>();
        services.AddScoped<IGenericRepository<TaskTagEntity>, GenericRepository<TaskTagEntity>>();
        services.AddScoped<IGenericRepository<TaskItemEntity>, TaskItemRepository>();
        services.AddScoped<IGenericRepository<TaskTagEntity>, TaskTagRepository>();

        services.AddScoped<IProjectsRepository, ProjectsRepository>();
        services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        services.AddScoped<ITaskTagRepository, TaskTagRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
