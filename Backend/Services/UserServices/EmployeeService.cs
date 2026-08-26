using AutoGovernance9Web.Backend.Data;
using AutoGovernance9Web.Backend.Dtos;
using Microsoft.AspNetCore.Connections;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AutoGovernance9Web.Backend.Services.UserServices
{
    public class EmployeeService
    {


        private readonly IDbConnectionInterface _connection;

        public EmployeeService(IDbConnectionInterface connection)
        {
            _connection = connection;
        }

        public async Task SubmitAssessmentAsync(AssessmentSubmissionDto dto)
        {
            using var conn = _connection.CreateConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                string insertSubmissionSql = @" INSERT INTO AssessmentSubmissions (TemplateId) VALUES (@TemplateId);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                int submissionId = await conn.ExecuteScalarAsync<int>(
                    insertSubmissionSql,
                    new { dto.TemplateId, dto.UserId },
                    transaction);

                string insertAnswerSql = @" INSERT INTO LikertAnswers (AssessmentSubmissionId, QuestionId, ScaleValue)  VALUES (@SubmissionId, @QuestionId, @LikertScore);";

                var answerRecords = dto.Answers.Select(a => new
                {
                    SubmissionId = submissionId,
                    QuestionId = a.QuestionId,
                    LikertScore = a.LikertScore
                });

                await conn.ExecuteAsync(insertAnswerSql, answerRecords, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}