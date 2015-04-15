namespace BlogSystem.Models
{
    using System.Collections.Generic;

    public class Comment
    {
        private ICollection<CommentLiker> commentLikers;

        public Comment()
        {
            this.CommentLikers = new HashSet<CommentLiker>();
        }

        public int Id { get; set; }

        public int UserName { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }

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
    }
}
