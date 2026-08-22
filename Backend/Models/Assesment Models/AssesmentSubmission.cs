namespace AutoGovernance9Web.Backend.Models.Assesment_Models
{
    public class AssesmentSubmission
    {
        public int AssesmentSubmissionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int TemplateId { get; set; }
        public bool IsFinalised { get; set; }
        public DateTime CompletedAt { get; set; }


    }
}
