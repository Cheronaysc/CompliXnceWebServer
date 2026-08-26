namespace AutoGovernance9Web.Backend.Models.Assesment_Models
{
    public class LikertAnswer
    {
        public int AnswerId { get; set; }
        public int AssessmentSubmissionId { get; set; }
        public int QuestionId { get; set; }
        public int ScaleValue { get; set; } 

        public string? Comment { get; set; }

        public LikertAnswer() { }

        public LikertAnswer(int answerId, int assessmentSubmissionId, int questionId, int scaleValue, string? comment = null)
        {
            AnswerId = answerId;
            AssessmentSubmissionId = assessmentSubmissionId;
            QuestionId = questionId;
            ScaleValue = scaleValue;
            Comment = comment;
        }
    }
}
