using CompliXnceWebApp.Backend.Services.UserServices;
namespace CompliXnceWebApp.Backend.Services.UserServices
{
    public class AuthenticationService
    {



        public bool RegisterNewUser()
        {
            return true;
        }

        public async Task<UserSession> AuthenticateUser(LogInDto logInDto) {
            {
                string session = "a";
                ///1.Query Db using dapper to find matching user
                //eg. SELECT userId, company, firstname, lastname, email, usertype
                //From users table
                //Where Email = @Email And Password = @Password

                // = wait (async) QueryFirstOrDefaultAsync <usr> (sql and log in dto)

                ///2.Check if user was found
                //if userRecord == null exc

                ///3.Create and start new session
                //var session = new Usersession


                //return session;

                //
            }
        }
    }
}
