using AutoGovernance9Web.Backend.Data;
using AutoGovernance9Web.Backend.Dtos;
using AutoGovernance9Web.Backend.Models;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace AutoGovernance9Web.Backend.Services.AssesmentServices
{
    public class AssessmentService
    {


        private readonly IDbConnectionInterface _connection;

        public AssessmentService(IDbConnectionInterface connection)
        {
            _connection = connection;
        }

        public async Task<AssessmentTemplateDto?> GetLatestTemplateAsync()
        {
            using var conn = _connection.CreateConnection();

            //select first record orginised by most recent
            string QueryGetLatest = @" SELECT TOP 1  TemplateId, UserId, TemplateTitle, Description, CreatedAt FROM AssessmentTemplates ORDER BY CreatedAt DESC;";

            return await conn.QueryFirstOrDefaultAsync<AssessmentTemplateDto>(QueryGetLatest);
        }

        public async Task<List<Question>> GetQuestionsForTemplateAsync(int templateId)
        {
            using var conn = _connection.CreateConnection();


            string sql = @"
                SELECT 
                    q.QuestionId, 
                    q.QuestionNumber, 
                    q.QuestionText, 
                    q.Framework, 
                    q.Domain
                FROM Questions q
                INNER JOIN JuncTemplateQuestions jq ON q.QuestionId = jq.QuestionId
                WHERE jq.TemplateId = @TemplateId
                ORDER BY q.QuestionNumber ASC;";

            var questions = await conn.QueryAsync<Question>(sql, new { TemplateId = templateId });
            return questions.ToList();
        }

        public async Task<AssessmentTemplateDto?> GetTemplateByIdAsync(int templateId)
        {
            using var conn = _connection.CreateConnection();
            string sql = @"SELECT TemplateId, UserId, TemplateTitle, Description, CreatedAt FROM AssessmentTemplates WHERE TemplateId = @TemplateId;";
            return await conn.QueryFirstOrDefaultAsync<AssessmentTemplateDto>(sql, new { TemplateId = templateId });
        }

        public async Task<List<AssessmentTemplateDto>> GetProposedAssessmentsForUserAsync(int userId)
        {
            using var conn = _connection.CreateConnection();

            string sql = @"
        SELECT 
            t.TemplateId, 
            t.UserId, 
            t.TemplateTitle, 
            t.Description, 
            t.CreatedAt
        FROM AssessmentTemplates t
        WHERE t.TemplateId NOT IN (
            SELECT s.TemplateId 
            FROM AssessmentSubmissions s 
            WHERE s.UserId = @UserId AND (s.IsFinalised = 1 OR s.Status = 'Completed')
        )
        ORDER BY t.CreatedAt DESC;";

            var templates = await conn.QueryAsync<AssessmentTemplateDto>(sql, new { UserId = userId });
            return templates.ToList();
        }

        public async Task<AssessmentScoreResultDto> CalculateAssessmentScoreAsync(int submissionId)
        {
            using var conn = _connection.CreateConnection();

            string sql = @" SELECT 
            d.DomainId,
            d.DomainName,
            AVG(CAST(a.ScaleValue AS FLOAT)) AS DomainAverage FROM AssessmentSubmissions s
            JOIN LikertAnswers a ON s.SubmissionId = a.SubmissionId
            JOIN Questions q ON a.QuestionId = q.QuestionId
            JOIN Domain d ON q.DomainId = d.DomainId
            WHERE s.SubmissionId = @SubmissionId
            GROUP BY d.DomainId, d.DomainName;";

            var domainResults = (await conn.QueryAsync<DomainScore>(sql, new { SubmissionId = submissionId })).ToList();

            if (!domainResults.Any())
            {
                return new AssessmentScoreResultDto();
            }

            // calculate the final maturity score as the average of all domain averages
            double finalScore = domainResults.Average(d => d.DomainAverage);

            return new AssessmentScoreResultDto
            {
                FinalMaturityScore = Math.Round(finalScore, 2),
                DomainScores = domainResults
            };
        }

        public async Task<AssessmentScoreResultDto> CalculateOrganizationMaturityAsync()
        {
            using var conn = _connection.CreateConnection(); 
            string sql = @"
        WITH AllDomainScores AS (
            SELECT 
                q.Domain AS DomainName,
                s.AssessmentSubmissionId,
                AVG(CAST(a.ScaleValue AS FLOAT)) AS SubmissionDomainAverage
            FROM AssessmentSubmissions s
            JOIN LikertAnswers a ON s.AssessmentSubmissionId = a.AssessmentSubmissionId
            JOIN Questions q ON a.QuestionId = q.QuestionId
            GROUP BY q.Domain, s.AssessmentSubmissionId
        )
        SELECT 
            ROW_NUMBER() OVER (ORDER BY DomainName) AS DomainId,
            DomainName,
            AVG(SubmissionDomainAverage) AS DomainAverage
        FROM AllDomainScores
        GROUP BY DomainName;";

            var domainResults = (await conn.QueryAsync<DomainScore>(sql)).ToList();

            if (!domainResults.Any())
            {
                return new AssessmentScoreResultDto();
            }

            // Average out all the domain averages to get the global enterprise maturity score
            double finalScore = domainResults.Average(d => d.DomainAverage);

            return new AssessmentScoreResultDto
            {
                FinalMaturityScore = Math.Round(finalScore, 2),
                DomainScores = domainResults
            };
        }

        public async Task CompleteAssessmentAsync(int assessmentSubmissionId)
        {
            using var conn = _connection.CreateConnection();

            string completeAssessment = @"UPDATE AssessmentSubmissions SET Status = 'Completed' 
            WHERE AssessmentSubmissionId = @AssessmentSubmissionId;";

            await conn.ExecuteAsync(completeAssessment, new { AssessmentSubmissionId = assessmentSubmissionId });
        }

        public async Task<SubmissionStatusDto?> GetSubmissionStatusAsync(int userId, int templateId)
        {
            using var conn = _connection.CreateConnection();

            string sql = @"SELECT TOP 1 AssessmentSubmissionId, IsFinalised, Status FROM AssessmentSubmissions
            WHERE UserId = @UserId AND TemplateId = @TemplateId
            ORDER BY AssessmentSubmissionId DESC;";

            return await conn.QueryFirstOrDefaultAsync<SubmissionStatusDto>(sql, new { UserId = userId, TemplateId = templateId });
        }

        public async Task<int> GetTotalSubmissionsCountAsync()
        {
            using var conn = _connection.CreateConnection();
            string sql = "SELECT COUNT(*) FROM AssessmentSubmissions;";
            return await conn.ExecuteScalarAsync<int>(sql);
        }

        public async Task<List<AssessmentSubmissionSummaryDto>> GetSubmissionsForAdminAsync()
        {
            using var conn = _connection.CreateConnection();

            string sql = @" SELECT
            s.AssessmentSubmissionId,
            s.TemplateId,
            t.TemplateTitle,
            s.UserId,
            (u.FirstName + ' ' + u.LastName) AS EmployeeName,
            s.IsFinalised,
            s.Status,
            s.CompletedAt
            FROM AssessmentSubmissions s
            JOIN AssessmentTemplates t ON t.TemplateId = s.TemplateId
            JOIN Users u ON u.UserId = s.UserId
            ORDER BY s.CompletedAt DESC;";

            var results = await conn.QueryAsync<AssessmentSubmissionSummaryDto>(sql);
            return results.ToList();
        }
    }
}
