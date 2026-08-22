using System.ComponentModel.Design;

namespace AutoGovernance9Web.Backend.Models
{
    // Base User class
    public enum UserType
    {
        Admin,
        Employee,
    }
    public enum UserStatus
    {
        Active,
        Terminated,
    }

    public class SignupRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public int? CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyKey { get; set; }

        public SignupRequest() { }

        public SignupRequest(string firstName, string lastName, string email, string phoneNumber, UserType userType, int? companyId = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            UserType = userType;
            CompanyId = companyId;
        }
    }

    public abstract class User
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public string PasswordHash { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public UserType UserType { get;private set; }
        public UserStatus UserStatus { get; private set; }
        public bool? IsActive { get; private set; }

        public User() { }

        public User(string firstName, string lastName, string email, string phoneNumber, UserType userType)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            UserType = userType;
            UserStatus = UserStatus.Active;
            IsActive = true;
        }
    }

    // Inherits from User
    public class Employee : User
    {
        public int EmployeeId { get; private set; }
        public DateTime? TerminationDate { get; set; }
        public string? TerminationReason { get; set; }

        public Employee() { }

        public Employee(int EmployeeId, DateTime? TerminationDate, string? TerminationReason):base()
        {
            this.EmployeeId = EmployeeId;
            this.TerminationDate = TerminationDate;
            this.TerminationReason = TerminationReason;
        }
    }

    public class Admin : User
    {
        public int AdminId { get; set; }
        public int CompanyId { get; private set; }
        public Admin() { }

        public Admin(string firstName, string lastName, string email, string phoneNumber, int companyId)
            : base(firstName, lastName, email, phoneNumber, UserType.Admin)
        {
            CompanyId = companyId;
        }


    }

}
