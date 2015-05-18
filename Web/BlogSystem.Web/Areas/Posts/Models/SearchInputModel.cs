namespace BlogSystem.Web.Areas.Posts.Models
{
    using BlogSystem.Models;

    public class SearchInputModel
    {
        public string SearchContent { get; set; }

        public PostSearchCategory? Category { get; set; }
    }
}