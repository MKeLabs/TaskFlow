using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaskFlow.DAL.Auth;
using TaskFlow.DAL.Context;
using TaskFlow.DAL.Entities;

namespace TaskFlow.API.Infrastructure;

public sealed class SeedAdminOptions
{
    public const string SectionName = "SeedAdmin";

    public bool Enabled { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var db = scope.ServiceProvider.GetRequiredService<TaskFlowDbContext>();

        await db.Database.MigrateAsync(cancellationToken);

        foreach (var roleName in new[] { AppRoles.Admin, AppRoles.User })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var seedOptions = scope.ServiceProvider.GetService<IOptions<SeedAdminOptions>>()?.Value;
        if (seedOptions is { Enabled: true, Email: not null, Password: not null } &&
            !string.IsNullOrWhiteSpace(seedOptions.Email) &&
            !string.IsNullOrWhiteSpace(seedOptions.Password))
        {
            var admin = await userManager.FindByEmailAsync(seedOptions.Email);
            if (admin is null)
            {
                admin = new ApplicationUser { UserName = seedOptions.Email, Email = seedOptions.Email, EmailConfirmed = true };
                var createResult = await userManager.CreateAsync(admin, seedOptions.Password);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AppRoles.Admin);
                }
            }
        }
    }
}
