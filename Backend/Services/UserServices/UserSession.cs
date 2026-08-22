using System.ComponentModel.DataAnnotations;

namespace AutoGovernance9Web.Backend.Services.UserServices
{
    namespace AutoGovernance9Web.Backend.Dtos
    {
        public class UserLoginDto
        {
            public int UserId { get; set; }
            public int CompanyId { get; set; }
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;
            public string UserType { get; set; } = string.Empty;
        }
    }
    public class UserSession
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string UserType { get; set; }

        public bool IsAuthorised => UserId > 0;

        public void EndSession()
        {
            UserId = 0;
            CompanyId = 0;
            FullName = string.Empty;
            Email = string.Empty;
            UserType = string.Empty;
        }
    }
}
