namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.Collections.Generic;

    public class LastCommentsPageViewModel
    {
        public int Page { get; set; }

        public IEnumerable<LastCommentViewModel> Comments { get; set; }
    }
}