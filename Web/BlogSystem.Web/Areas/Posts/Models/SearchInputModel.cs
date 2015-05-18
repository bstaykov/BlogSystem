namespace BlogSystem.Web.Areas.Posts.Models
{
    using BlogSystem.Models;

    public class SearchInputModel
    {
        public string SearchContent { get; set; }

        public PostCategory? Category { get; set; }

        public string SearchAuthor { get; set; }
    }
}