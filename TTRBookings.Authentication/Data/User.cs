using TTRBookings.Core;

namespace TTRBookings.Authentication.Data
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }

        private User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public static User Create(string name, string password)
        {
            return new User(name, Encryption.HashPassword(password));
        }
    }
}
