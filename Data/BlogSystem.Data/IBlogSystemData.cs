namespace BlogSystem.Data
{
    using BlogSystem.Common.Repository;
    using BlogSystem.Models;

    public interface IBlogSystemData
    {
        IRepository<User> Users { get; }

        IRepository<Comment> Comments { get; }

        IRepository<CommentLiker> CommentLikers { get; }

        IRepository<Log> Logs { get; }

        IRepository<Post> Posts { get; }

        IRepository<PostLiker> PostLikers { get; }

        IRepository<Tag> Tags { get; }

        int SaveChanges();
    }
}
