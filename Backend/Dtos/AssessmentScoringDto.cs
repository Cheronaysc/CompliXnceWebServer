namespace AutoGovernance9Web.Backend.Dtos
{
    public class AssessmentScoreResultDto
    {
        public double FinalMaturityScore { get; set; }
        public List<DomainScore> DomainScores { get; set; } = new();
    }

    public class DomainScore
    {
        public int DomainId { get; set; }
        public string DomainName { get; set; }
        public int DomainAverage { get; set; }
    }
}

