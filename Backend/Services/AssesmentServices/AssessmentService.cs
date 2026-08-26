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
            WHERE s.UserId = @UserId AND s.IsFinalised = 1
        )
        ORDER BY t.CreatedAt DESC;";

            var templates = await conn.QueryAsync<AssessmentTemplateDto>(sql, new { UserId = userId });
            return templates.ToList();
        }


    }
}
