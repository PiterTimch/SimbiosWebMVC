namespace SimbiosWebMVC.Models.Helpers
{
    public class PaginationViewModel
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int ItemsPerPage { get; set; } = 5;
        public int TotalPages { get; set; }

        public List<int> Elements { get; set; } = new() { 5, 10, 15, 20 };
    }
}
