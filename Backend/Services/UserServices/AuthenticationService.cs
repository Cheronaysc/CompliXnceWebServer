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
using AutoGovernance9Web.Backend.Services;
namespace AutoGovernance9Web.Backend.Services.UserServices


{
    public class AuthenticationService
    {
        private readonly IDbConnectionInterface _connection;
        private readonly UserSession _userSession;

        public AuthenticationService(IDbConnectionInterface connection, UserSession userSession)
        {
            _connection = connection;
            _userSession = userSession;
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
                    UserType = signupRequest.UserType.ToString(),
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

        public async Task<string?> RegisterEmployeeAsync(SignupRequest signupRequest)
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

                //look up company
                var companyId = await conn.ExecuteScalarAsync<int?>(
                    "SELECT CompanyId FROM Companies WHERE CompanyKey = @CompanyKey",
                    new { CompanyKey = signupRequest.CompanyKey },
                    transaction);

                if (companyId == null)
                {
                    return "No company found with that company key. Please ask your admin for a key.";
                }
                 
                // Insert User
                var userSql = @"  INSERT INTO Users (FirstName, LastName, Email, PasswordHash, UserType, CompanyId)
                VALUES (@FirstName, @LastName, @Email, @PasswordHash, @UserType, @CompanyId);";

                var newUserParams = new
                {
                    FirstName = signupRequest.FirstName,
                    LastName = signupRequest.LastName,
                    Email = signupRequest.Email,
                    PasswordHash = passwordHash,
                    UserType = signupRequest.UserType.ToString(),
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

        public async Task<(UserLoginDto? User, string? ErrorMessage)> LoginAsync(LoginRequest loginRequest)
        {
            using var conn = _connection.CreateConnection();

            try
            {
                var user = await conn.QuerySingleOrDefaultAsync<UserLoginDto>(
                    "SELECT UserId, CompanyId, FirstName, LastName, Email, PasswordHash, UserType FROM Users WHERE Email = @email",
                    new { email = loginRequest.Email });

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
                {
                    return (null, "Invalid email or password.");
                }

                return (user, null);
            }
            catch (Exception ex)
            {
                return (null, "Login failed. Please try again.");
            }
        }

    }
}




