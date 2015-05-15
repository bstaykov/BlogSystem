namespace BlogSystem.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Post> posts;
        private ICollection<Log> logs;
        private ICollection<PostLiker> postLikers;
        private ICollection<PostReader> postReaders;
        private ICollection<CommentLiker> commentLikers;
        private ICollection<Comment> comments;

        public User()
        {
            this.Posts = new HashSet<Post>();
            this.Logs = new HashSet<Log>();
            this.PostLikers = new HashSet<PostLiker>();
            this.PostReaders = new HashSet<PostReader>();
            this.CommentLikers = new HashSet<CommentLiker>();
            this.Comments = new HashSet<Comment>();
        }

        public string ImageUrl { get; set; }

        public int Points { get; set; }

        public virtual ICollection<Post> Posts
        {
            get
            {
                return this.posts;
            }

            set
            {
                this.posts = value;
            }
        }

        public virtual ICollection<Log> Logs
        {
            get
            {
                return this.logs;
            }

            set
            {
                this.logs = value;
            }
        }

        public virtual ICollection<PostLiker> PostLikers
        {
            get
            {
                return this.postLikers;
            }

            set
            {
                this.postLikers = value;
            }
        }

        public virtual ICollection<PostReader> PostReaders
        {
            get
            {
                return this.postReaders;
            }

            set
            {
                this.postReaders = value;
            }
        }

        public virtual ICollection<CommentLiker> CommentLikers
        {
            get
            {
                return this.commentLikers;
            }

            set
            {
                this.commentLikers = value;
            }
        }

        public virtual ICollection<Comment> Comments
        {
            get
            {
                return this.comments;
            }

            set
            {
                this.comments = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            return userIdentity;
        }
    }
}
