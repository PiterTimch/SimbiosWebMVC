namespace SimbiosWebMVC.Models.Helpers
{
    public class PaginationViewModel
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalPages { get; set; }
    }
}
