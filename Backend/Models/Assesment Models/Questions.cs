namespace CompliXnceWebApp.Backend.Models
{
    public enum Framework
    {
        COBIT2019,
        ISO38000,
    }
    public enum Domain
    {
        GovernanceAndStrategy,
        RiskManagement,
        SDLC, AndChangeManagement,
        EmployeeSatisfaction,
        Ethics,
    }
    public class Question
        {
            public int QuestionId { get; set; }
            public string QuestionText { get; set; }
            public int QuestionNumber { get; set; }
            public Framework Framework { get; set; }
            public Domain Domain { get; set; }
            public int Weight { get; set; }

            public Question()
            {

            }

            public Question(int questionId, string questionText, int questionNumber, Framework framework, Domain domain, int weight)
            {
                QuestionId = questionId;
                QuestionText = questionText;
                QuestionNumber = questionNumber;
                Framework = framework;
                Domain = domain;
                Weight = weight;
            }
        }
    }

