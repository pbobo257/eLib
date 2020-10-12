using System;

namespace eLib.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        User,
        Admin
    }
}
