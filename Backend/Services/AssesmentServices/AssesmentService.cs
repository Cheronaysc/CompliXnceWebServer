using AutoGovernance9Web.Backend.Models;
using AutoGovernance9Web.Backend.Models.Assesment_Models;
namespace AutoGovernance9Web.Backend.Services.AssesmentServices
{
    public class AssesmentService
    {
        //connection string 


        public async Task<int> SubmitSingleAssesmentAsync(int companyId, int userId, int templateId, List<LikertAnswer> answers)
        {
            ///1. Calculate total score. use LINQ or Foreach??
            int totalscore = 0;
            // foreach answer in answers
            //{calculated score += answer.ScaleValue

            return totalscore;
        }

        public string GetMaturityRating(int totalscore)
        {
            //For example, use switch or if else statements to define score level
            if (totalscore > 81)
            {
                {
                    return "Level 5- Optimized";
                }
            }
            else
            {
                return string.Empty;
            }

        }

    }
}

