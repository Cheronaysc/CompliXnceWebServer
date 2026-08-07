namespace CompliXnceWebApp.Backend.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyKey { get; set; }

        public List<Employee>?Employees { get; set;  } = new List<Employee>();


        public Company() { }
        public Company(int CompanyId, string CompanyName, string CompanyAddress, string CompanyKey)
        {
            this.CompanyId = CompanyId;
            this.CompanyName = CompanyName;
            this.CompanyAddress = CompanyAddress;
            this.CompanyKey = CompanyKey;
        }
    }



}
