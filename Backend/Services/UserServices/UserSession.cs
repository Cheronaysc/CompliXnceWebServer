using System.ComponentModel.DataAnnotations;

namespace CompliXnceWebApp.Backend.Services.UserServices
{
    //use a dto for log ins
    public class LogInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LogInDto() { }
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
