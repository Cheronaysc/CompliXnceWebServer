using AutoGovernance9Web.Backend.Models;

namespace AutoGovernance9Web.Backend.Dtos

{
    //questions shall not be seeded to the db, they act only as part of an assesment template
    public class AssessmentTemplateDto
    {
        public int UserId { get; set; }
        public int TemplateId { get; set; }
        public string TemplateTitle { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<int> QuestionIds { get; set; } = new();

        public AssessmentTemplateDto() { }
    }
}


public class QuestionIds
    {
        public string QuestionText { get; set; }
        public int QuestionNumber { get; set; }
        public string Framework { get; set; }
        public string Domain { get; set; }
    

    public QuestionIds() { }
    }
