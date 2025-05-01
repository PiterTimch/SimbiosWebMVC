namespace SimbiosWebMVC.Areas.Admin.Models.User
{
    public class UserItemViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
