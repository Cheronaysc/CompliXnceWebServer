namespace AutoGovernance9Web.Backend.Models
{


    public class AssesmentTemplate
    {
        public int TemplateId { get; set; }
        public int CompanyId { get; set; }
        public int MaxQuestions { get; set; }
        public List<Question> ListOfQuestions { get; set; } = new List<Question>();


        public AssesmentTemplate()
        {

        }

        public AssesmentTemplate(int templateId, int companyId, int maxQuestions, List<Question> listOfQuestions)
        {
            TemplateId = templateId;
            CompanyId = companyId;
            MaxQuestions = maxQuestions;
            ListOfQuestions = listOfQuestions;
        }


    }
}
