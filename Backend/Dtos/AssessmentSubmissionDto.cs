namespace AutoGovernance9Web.Backend.Dtos
{
    public class AssessmentSubmissionDto
    {
        public int UserId { get; set; }
        public int TemplateId { get; set; }
        public List<LikertAnswerDto> Answers { get; set; } = new();
    }

    public class LikertAnswerDto
    {
        public int QuestionId { get; set; }
        public int LikertScore { get; set; }
    }

    //check submission status
    public class SubmissionStatusDto
    {
        public int AssessmentSubmissionId { get; set; }
        public bool IsFinalised { get; set; }
        public string? Status { get; set; }

        public bool IsLocked => IsFinalised || Status == "Completed";
    }

    public class AssessmentSubmissionSummaryDto
    {
        public int AssessmentSubmissionId { get; set; }
        public int TemplateId { get; set; }
        public string TemplateTitle { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public bool IsFinalised { get; set; }
        public string? Status { get; set; }

    }
}

