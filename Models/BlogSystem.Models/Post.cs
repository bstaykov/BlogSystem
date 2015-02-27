namespace BlogSystem.Models
{
    using System;
    using System.Collections.Generic;

    public class Post
    {
        private ICollection<PostLiker> postLikers;
        private ICollection<Comment> comments;
        private ICollection<Tag> tags;

        public Post()
        {
            this.postLikers = new HashSet<PostLiker>();
            this.comments = new HashSet<Comment>();
            this.tags = new HashSet<Tag>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public PostCategory Category { get; set; }

        public DateTime DateTimePosted { get; set; }

        public string Author { get; set; }

        public int Likes { get; set; }

        public int CommentsCount { get; set; }

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

        public virtual ICollection<Tag> Tags
        {
            get
            {
                return this.tags;
            }
            set
            {
                this.tags = value;
            }
        }
    }
}
