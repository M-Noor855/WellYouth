using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WellYouth.Models.Entities;

namespace WellYouth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HealthHabit> HealthHabits => Set<HealthHabit>();
        public DbSet<HealthHabitLog> HealthHabitLogs => Set<HealthHabitLog>();
        public DbSet<HealthStat> HealthStats => Set<HealthStat>();
        public DbSet<HealthScoreEntry> HealthScoreEntries => Set<HealthScoreEntry>();
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<ArticleCategory> ArticleCategories => Set<ArticleCategory>();
        public DbSet<Specialist> Specialists => Set<Specialist>();
        public DbSet<SpecialistContactRequest> SpecialistContactRequests => Set<SpecialistContactRequest>();
        public DbSet<CommunityGroup> CommunityGroups => Set<CommunityGroup>();
        public DbSet<CommunityGroupMember> CommunityGroupMembers => Set<CommunityGroupMember>();
        public DbSet<CommunityPost> CommunityPosts => Set<CommunityPost>();
        public DbSet<AssistantConversation> AssistantConversations => Set<AssistantConversation>();
        public DbSet<AssistantMessage> AssistantMessages => Set<AssistantMessage>();
        public DbSet<WellnessActivity> WellnessActivities => Set<WellnessActivity>();
        public DbSet<UserActivityLog> UserActivityLogs => Set<UserActivityLog>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Additional configurations if needed
            
            // Example: Unique constraint on Article Slug
            builder.Entity<Article>()
                .HasIndex(a => a.Slug)
                .IsUnique();

            // Example: Unique constraint on Community Group Name
            builder.Entity<CommunityGroup>()
                .HasIndex(cg => cg.Name)
                .IsUnique();

            // One habit log per habit per date
            builder.Entity<HealthHabitLog>()
                .HasIndex(hl => new { hl.HealthHabitId, hl.LogDate })
                .IsUnique();

            // Specialist specialty indexed
            builder.Entity<Specialist>()
                .HasIndex(s => s.Specialty);
                
            // Disable cascading deletes globally to prevent cycle errors
            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
