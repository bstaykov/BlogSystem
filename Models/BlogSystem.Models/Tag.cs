namespace BlogSystem.Models
{
    using System.Collections.Generic;

    public class Tag
    {
        private ICollection<Post> posts;

        public Tag()
        {
            this.Posts = new HashSet<Post>();
        }

        public int Id { get; set; }

        public int PostId { get; set; }

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
    }
}