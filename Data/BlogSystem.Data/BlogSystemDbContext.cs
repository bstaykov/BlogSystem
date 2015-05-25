namespace BlogSystem.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using BlogSystem.Common.Models;
    using BlogSystem.Data.Migrations;
    using BlogSystem.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class BlogSystemDbContext : IdentityDbContext<User>
    {
        public BlogSystemDbContext()
            : base("BlogSystem", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogSystemDbContext, Configuration>());
        }

        public IDbSet<Post> Posts { get; set; }

        public IDbSet<Log> Logs { get; set; }

        public IDbSet<Tag> Tags { get; set; }

        public IDbSet<Message> Messages { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<PostLiker> PostLikers { get; set; }

        public IDbSet<PostReader> PostReaders { get; set; }

        public IDbSet<CommentLiker> CommentLikers { get; set; }

        public override IDbSet<User> Users { get; set; }

        public static BlogSystemDbContext Create()
        {
            return new BlogSystemDbContext();
        }

        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }

        private void ApplyAuditInfoRules()
        {
            // Approach via @julielerman: http://bit.ly/123661P
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditInfo && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditInfo)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    if (!entity.PreserveCreatedOn)
                    {
                        entity.CreatedOn = DateTime.Now;
                    }
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
    }
}
