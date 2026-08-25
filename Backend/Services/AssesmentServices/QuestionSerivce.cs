using AutoGovernance9Web.Backend.Models;
using Microsoft.AspNetCore.Connections;
using AutoGovernance9Web.Backend.Data;
using Dapper;
using System.Security.Cryptography.X509Certificates;

namespace AutoGovernance9Web.Backend.Services.AssesmentServices
{
    public class QuestionService
    {
        private readonly IDbConnectionInterface _connection;

        public QuestionService(IDbConnectionInterface connection)
        {
            _connection = connection;
        }
        public QuestionService() { }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            using var conn = _connection.CreateConnection();
            const string sql = @"SELECT QuestionId, QuestionNumber, QuestionText, Framework, Domain FROM Questions;";

            var rows = await conn.QueryAsync<QuestionRow>(sql);

            return rows.Select(r => new Question(
                r.QuestionId,
                r.QuestionText,
                r.QuestionNumber,
                //store as strings
                Enum.Parse<Framework>(r.Framework),
                Enum.Parse<Domain>(r.Domain)
            )).ToList();

            
        }

        private class QuestionRow
        {
            public int QuestionId { get; set; }
            public int QuestionNumber { get; set; }
            public string QuestionText { get; set; } = string.Empty;
            public string Framework { get; set; } = string.Empty;
            public string Domain { get; set; } = string.Empty;
        }
    }
}