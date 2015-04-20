namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.Collections.Generic;

    public class MyPostsPagingViewModel
    {
        public IEnumerable<MyPostViewModel> Posts { get; set; }

        public int StartPage { get; set; }

        public int CurrentPage { get; set; }

        public int EndPage { get; set; }
    }
}