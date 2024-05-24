using Shared.Models.DesafioFullStack;

namespace WebPage.DesafioFullStack.Models
{
    public class UserManagementPageModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Profile { get; set; }
        public bool IsCreated { get; set; }
        public List<User> ListSelectUsers { get; set; }
        public string SelectedUser { get; set; }

    }
}
