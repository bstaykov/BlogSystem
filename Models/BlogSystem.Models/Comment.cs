using System.Collections.Generic;
namespace BlogSystem.Models
{
    public class Comment
    {
        private ICollection<CommentLiker> commentLikers;

        public Comment()
        {
            this.commentLikers = new HashSet<CommentLiker>();
        }


        public int Id { get; set; }

        public int UserName { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int PostId { get; set; }

        public virtual Post post { get; set; }

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
