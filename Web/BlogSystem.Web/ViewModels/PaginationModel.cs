namespace BlogSystem.Web.ViewModels
{
    using System;
    using System.Collections.Generic;

    using BlogSystem.Models;

    public class PaginationModel
    {
        public string AreaName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public int StartPage { get; set; }

        public int CurrentPage { get; set; }

        public int EndPage { get; set; }

        public int AvailablePages { get; set; }

        public string SearchContent { get; set; }

        public PostCategory? Category { get; set; }

        public string SearchAuthor { get; set; }

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }

        public string UpdateTarget { get; set; }
    }
}