namespace ElixinBackend.Users.UseCases
{
    public class UserView
    {
        public string Username { get; set; }

        public static UserView FromUser(User user)
        {
            return new UserView()
            {
                Username = user.Username
            };
        }
    }
}