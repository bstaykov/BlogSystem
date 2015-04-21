namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.Collections.Generic;

    using BlogSystem.Web.ViewModels;

    public class MyPostsPagingViewModel
    {
        public IEnumerable<MyPostViewModel> Posts { get; set; }

        public PaginationModel Pagination { get; set; }
    }
}