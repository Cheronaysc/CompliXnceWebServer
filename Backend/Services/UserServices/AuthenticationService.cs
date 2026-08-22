using AutoGovernance9Web.Backend.Data;
using AutoGovernance9Web.Backend.Dtos;
using AutoGovernance9Web.Backend.Models;
using AutoGovernance9Web.Backend.Services.UserServices;
using AutoGovernance9Web.Backend.Services.UserServices.AutoGovernance9Web.Backend.Dtos;
using AutoGovernance9Web.Components.Pages;
using BCrypt;
using Dapper;
using Microsoft.AspNetCore.Connections;
using System.Security.Cryptography.X509Certificates;
namespace AutoGovernance9Web.Backend.Services.UserServices

{
    public class AuthenticationService
    {
        private readonly IDbConnectionInterface _connection;
        private readonly UserSession _userSession;

        public AuthenticationService(IDbConnectionInterface connection)
        {
            _connection = connection;
        }



        public async Task<string?> RegisterAdminAsync(SignupRequest signupRequest)
        {
            using var conn = _connection.CreateConnection();

            using var transaction = conn.BeginTransaction();
            try
            {
                var emailExists = await conn.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM Users WHERE Email = @email",
                    new { email = signupRequest.Email },
                    transaction);

                if (emailExists > 0)
                {
                    return "That email is already registered. Please sign in.";
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(signupRequest.Password);

                // Insert Company
                var companySql = @"
            INSERT INTO Companies (CompanyName, CompanyKey)
            OUTPUT INSERTED.CompanyId
            VALUES (@CompanyName, @CompanyKey);";

                var companyId = await conn.ExecuteScalarAsync<int>(
                    companySql,
                    new { CompanyName = signupRequest.CompanyName, CompanyKey = signupRequest.CompanyKey },
                    transaction);

                // Insert User
                var userSql = @"
            INSERT INTO Users (FirstName, LastName, Email, PasswordHash, UserType, CompanyId)
            VALUES (@FirstName, @LastName, @Email, @PasswordHash, @UserType, @CompanyId);";

                var newUserParams = new
                {
                    FirstName = signupRequest.FirstName,
                    LastName = signupRequest.LastName,
                    Email = signupRequest.Email,
                    PasswordHash = passwordHash,
                    UserType = signupRequest.UserType,
                    CompanyId = companyId
                };

                await conn.ExecuteAsync(userSql, newUserParams, transaction);

                transaction.Commit();
                return null; // success
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"[RegisterAdmin Error]: {ex.Message}");
                return $"Registration failed: {ex.Message}";
            }



        }
    }


}?@


