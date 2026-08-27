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
                //Revoke permission of an employee trying to take the same assessment twice
                string alreadySubmittedSql = @" SELECT COUNT(1) FROM AssessmentSubmissions WHERE UserId = @UserId AND TemplateId = @TemplateId AND IsFinalised = 1;";

                int existingCount = await conn.ExecuteScalarAsync<int>(
                    alreadySubmittedSql,
                    new { dto.UserId, dto.TemplateId },
                    transaction);

                if (existingCount > 0)
                {
                    throw new InvalidOperationException("You have already submitted this assessment.");
                }

                string insertSubmissionSql = @" INSERT INTO AssessmentSubmissions (UserId, TemplateId, IsFinalised, CompletedAt) VALUES (@UserId, @TemplateId, 1, GETUTCDATE());
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