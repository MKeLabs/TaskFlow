using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.Entities;

namespace TaskFlow.DAL.Context;

public class TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<TaskItemEntity> TaskItems => Set<TaskItemEntity>();
    public DbSet<TaskCommentEntity> TaskComments => Set<TaskCommentEntity>();
    public DbSet<TaskTagEntity> TaskTags => Set<TaskTagEntity>();
    public DbSet<TaskItemTagEntity> TaskItemTags => Set<TaskItemTagEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectEntity>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<TaskItemEntity>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(2000);
            entity.HasOne(x => x.Project)
                .WithMany(x => x.TaskItems)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskCommentEntity>(entity =>
        {
            entity.Property(x => x.Text).HasMaxLength(2000).IsRequired();
            entity.HasOne(x => x.TaskItem)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.TaskItemId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskTagEntity>(entity =>
        {
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<TaskItemTagEntity>(entity =>
        {
            entity.HasKey(x => new { x.TaskItemId, x.TaskTagId });
            entity.HasOne(x => x.TaskItem)
                .WithMany(x => x.TaskItemTags)
                .HasForeignKey(x => x.TaskItemId);
            entity.HasOne(x => x.TaskTag)
                .WithMany(x => x.TaskItemTags)
                .HasForeignKey(x => x.TaskTagId);
        });
    }
}
