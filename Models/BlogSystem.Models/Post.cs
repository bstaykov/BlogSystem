namespace BlogSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using BlogSystem.Common.Models;

    public class Post : DeletableEntity
    {
        private ICollection<PostLiker> postLikers;
        private ICollection<PostReader> postReaders;
        private ICollection<Comment> comments;
        private ICollection<Tag> tags;

        public Post()
        {
            this.PostLikers = new HashSet<PostLiker>();
            this.PostReaders = new HashSet<PostReader>();
            this.Comments = new HashSet<Comment>();
            this.Tags = new HashSet<Tag>();
        }

        public int Id { get; set; }

        [MaxLength(50)]
        [Index("UniqueTitle", 1, IsUnique = true)]
        public string Title { get; set; }

        public string Content { get; set; }

        public PostCategory Category { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int Likes { get; set; }

        public int CommentsCount { get; set; }

        public int TimesRead { get; set; }

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
