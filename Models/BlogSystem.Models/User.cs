namespace BlogSystem.Models
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class User : IdentityUser
    {
        private ICollection<PostLiker> postLikers;
        private ICollection<CommentLiker> commentLikers;

        public User()
        {
            this.postLikers = new HashSet<PostLiker>();
            this.commentLikers = new HashSet<CommentLiker>();
        }

        public string ImageUrl { get; set; }

        public int Points { get; set; }

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
