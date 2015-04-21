namespace BlogSystem.Web.ViewModels
{
    public class PaginationModel
    {
        public string AreaName { get; set; }

        public string ControllerName { get; set; }

        public string ViewName { get; set; }

        public int StartPage { get; set; }

        public int CurrentPage { get; set; }

        public int EndPage { get; set; }
    }
}