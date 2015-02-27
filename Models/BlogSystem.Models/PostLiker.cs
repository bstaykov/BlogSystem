namespace BlogSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PostLiker
    {
        [Key, Column(Order = 0)]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsLiked { get; set; }
    }
}
