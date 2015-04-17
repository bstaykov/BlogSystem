namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.Collections.Generic;

    public class PostsPagingViewModel
    {
        public IEnumerable<PostViewModel> Posts { get; set; }

        public int StartPage { get; set; }

        public int CurrentPage { get; set; }

        public int EndPage { get; set; }
    }
}