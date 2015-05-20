namespace BlogSystem.Models
{
    using System;
    using System.Collections.Generic;

    using BlogSystem.Common.Models;

    public class Comment : DeletableEntity
    {
        private ICollection<CommentLiker> commentLikers;
        private ICollection<Comment> replyComments;

        public Comment()
        {
            this.CommentLikers = new HashSet<CommentLiker>();
            this.ReplyComments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public int? ParentCommentId { get; set; }

        public virtual Comment ParentComment { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int ReplyCommentsCount { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public bool IsReadByAuthor { get; set; }

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

        public virtual ICollection<Comment> ReplyComments
        {
            get
            {
                return this.replyComments;
            }

            set
            {
                this.replyComments = value;
            }
        }
    }
}
