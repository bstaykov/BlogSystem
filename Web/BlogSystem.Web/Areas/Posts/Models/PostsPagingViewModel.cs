namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.Collections.Generic;

    using BlogSystem.Web.ViewModels;

    public class PostsPagingViewModel
    {
        public IEnumerable<PostListViewModel> Posts { get; set; }

        public PaginationModel Pagination { get; set; }
    }
}