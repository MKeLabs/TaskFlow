using Microsoft.Extensions.DependencyInjection;
using TaskFlow.BLL.Services.Implementations;
using TaskFlow.BLL.Services.Interfaces;

namespace TaskFlow.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ITaskTagService, TaskTagService>();
        services.AddScoped<ITaskCommentService, TaskCommentService>();
        services.AddScoped<ITaskItemService, TaskItemService>();
        return services;
    }
}
