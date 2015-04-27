namespace BlogSystem.Models
{
    using System;
    using System.Collections.Generic;

    public class Comment
    {
        private ICollection<CommentLiker> commentLikers;
        private ICollection<Comment> subComments;

        public Comment()
        {
            this.CommentLikers = new HashSet<CommentLiker>();
            this.SubComments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public int UserName { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int SubCommentsCount { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

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

        public virtual ICollection<Comment> SubComments
        {
            get
            {
                return this.subComments;
            }

            set
            {
                this.subComments = value;
            }
        }
    }
}
