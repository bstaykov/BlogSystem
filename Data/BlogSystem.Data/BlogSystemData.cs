namespace BlogSystem.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using BlogSystem.Common.Repository;
    using BlogSystem.Models;

    public class BlogSystemData : IBlogSystemData
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public BlogSystemData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<User> Users
        {
            get { return this.GetRepository<User>(); }
        }

        public IRepository<Message> Messages
        {
            get { return this.GetRepository<Message>(); }
        }

        public IRepository<Comment> Comments
        {
            get { return this.GetRepository<Comment>(); }
        }

        public IRepository<CommentLiker> CommentLikers
        {
            get { return this.GetRepository<CommentLiker>(); }
        }

        public IRepository<Log> Logs
        {
            get { return this.GetRepository<Log>(); }
        }

        public IRepository<Post> Posts
        {
            get { return this.GetRepository<Post>(); }
        }

        public IRepository<PostLiker> PostLikers
        {
            get { return this.GetRepository<PostLiker>(); }
        }

        public IRepository<PostReader> PostReaders
        {
            get { return this.GetRepository<PostReader>(); }
        }

        public IRepository<Tag> Tags
        {
            get { return this.GetRepository<Tag>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(GenericRepository<T>), this.context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
