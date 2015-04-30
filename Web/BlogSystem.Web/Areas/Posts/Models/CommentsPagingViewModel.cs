namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.Collections.Generic;

    using BlogSystem.Web.ViewModels;

    public class CommentsPagingViewModel
    {
        public IEnumerable<CommentViewModel> Comments { get; set; }

        public PaginationModel Pagination { get; set; }
    }
}