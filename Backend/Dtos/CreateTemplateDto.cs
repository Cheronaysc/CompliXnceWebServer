
using CompliXnceWebApp.Backend.Models;

namespace CompliXnceWebApp.Backend.Dtos

{
    public class CreateTemplateDto
    {
        public int CompanyId { get; set; }
        public int MaxQuestions { get; set; }
        public List <QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public string QuestionText { get; set; }
        public int QuestionNumber { get; set; }
        public string Framework { get; set; }

        public string Domain { get; set; }
    }
    
}
