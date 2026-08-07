namespace CompliXnceWebApp.Backend.Models.Assesment_Models
{
    public class LikertAnswer
    {
        public int AnswerId { get; set; }
        public int AssessmentSubmissionId { get; set; }
        public int QuestionId { get; set; }
        public int ScaleValue { get; set; } 

        public string? Comment { get; set; }

        public LikertAnswer() { }

        public LikertAnswer(int AnswerId, int AssesmentId, int QuestionId, int ScaleValue, string? Comment)
        {

        }
    }
}
