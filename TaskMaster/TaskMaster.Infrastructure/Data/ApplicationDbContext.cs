using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskMaster.Domain.Common;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>,int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // every DbSet is Table in database
        // public DbSet<User> Users { get; set; } // Have this integrated in 
        public DbSet<Project> Projects { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<TokenBlackList> TokenBlacklists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ENTITIES CONFIGURATION

            // task 

            modelBuilder.Entity<Domain.Entities.Task>(entity =>
            {
                // task has one project
                entity.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);// delete tasks when project deleted
                // task has one creator
                entity.HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); // not allowing user to be deleted if they created task
                // task has one assigne
                entity.HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull); // set to null if assignee deleted
            }
            );

            modelBuilder.Entity<Project>(entity =>
            {
                // project has one owner
                entity.HasOne(p => p.Owner)
                .WithMany(u => u.OwnedProjects)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // not allowing user to be deleted if they own project
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                // project member has one project
                entity.HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // delete projectMember when project deleted
                entity.HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMemberships)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // delete projectMember when user deleted

                // user can not be added to same project twice - unique constraint
                entity.HasIndex(pm => new { pm.ProjectId, pm.UserId }).IsUnique();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // when user deleted do not delete comments

                entity.HasOne(c => c.Task)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasOne(a => a.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.UploadedBy)
                .WithMany(u => u.Attachments)
                .HasForeignKey(a => a.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure activity log entity
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.HasOne(al => al.User)
                .WithMany(u => u.ActivityLogs)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<TokenBlackList>(entity =>
            {
                entity.HasIndex(t => t.TokenId).IsUnique();
                entity.HasIndex(t => t.ExpiresAt);
            });
        }


        // SaveChanges automatically set CreatedAt/UpdatedAt
        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return await base.SaveChangesAsync(cancellationToken);
        }
        private void SetAuditFields()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries()
           .Where(e => e.Entity is BaseEntity &&
                      (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (EntityEntry entry in entries)
            {
                BaseEntity entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
                // i am using UtcNow because i do not have to worry about timezones (yet)
            }
        }
    }
}
