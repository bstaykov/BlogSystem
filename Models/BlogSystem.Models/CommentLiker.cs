namespace BlogSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CommentLiker
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public bool IsLiked { get; set; }
    }
}
