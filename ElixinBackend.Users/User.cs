using ElixinBackend.Shared;

namespace ElixinBackend.Users
{
    public class User : BaseEntity
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public bool IsSame(User other)
        {
            return Username == other.Username;
        }
    }
}