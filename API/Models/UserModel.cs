using System;

namespace API.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }

        public string Token { get; set; }

        public string DisplayName { get; set; }
    }
}