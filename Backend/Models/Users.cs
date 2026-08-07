namespace CompliXnceWebApp.Backend.Models
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
    public abstract class User
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public UserType UserType { get;private set; }
        public UserStatus UserStatus { get; private set; }
        public bool? IsActive { get; private set; }

        public User() { }
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
        public Admin() { }

        public Admin(int AdminId) : base()
        {
            this.AdminId = AdminId;
        }


    }

}
