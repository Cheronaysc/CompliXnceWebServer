using AutoGovernance9Web.Backend.Data;
using AutoGovernance9Web.Backend.Dtos;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AutoGovernance9Web.Backend.Services.UserServices
{
    public class AdminService
    {
        private readonly IDbConnectionInterface _connection;

        public AdminService(IDbConnectionInterface connection)
        {
            _connection = connection;
        }

        public async Task<int> CreateTemplateAsync(AssessmentTemplateDto dto)
        {
            if (dto.QuestionIds == null || dto.QuestionIds.Count != 20)
                throw new ArgumentException("A template must contain exactly 20 questions.");

            using var conn = _connection.CreateConnection();
            if (conn.State != ConnectionState.Open)
                conn.Open();

            // transaction allows multiple operations, but it must be rolled back
            using var transaction = conn.BeginTransaction();

            try
            {
                string insertTemplateSql = @"
            INSERT INTO AssessmentTemplates (UserId, TemplateTitle, Description, CreatedAt)  VALUES (@UserId, @TemplateTitle, @Description, GETUTCDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);";

                int templateId = await conn.ExecuteScalarAsync<int>(
                    insertTemplateSql,
                    new { dto.UserId, dto.TemplateTitle, dto.Description },
                    transaction);

                var junctionRecords = dto.QuestionIds.Select(qId => new
                {
                    TemplateId = templateId,
                    QuestionId = qId
                });
                //put the template id and question id into a junction table, create juncion
                string insertintoJunctionSql = @"  INSERT INTO JuncTemplateQuestions (TemplateId, QuestionId)
                VALUES (@TemplateId, @QuestionId);";

                await conn.ExecuteAsync(insertintoJunctionSql, junctionRecords, transaction);

                transaction.Commit();
                return templateId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}

