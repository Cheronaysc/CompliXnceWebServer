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
}

