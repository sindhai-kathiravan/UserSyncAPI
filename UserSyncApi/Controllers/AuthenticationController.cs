using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using UserSyncApi.Helpers;
using UserSyncApi.Models;

namespace UserSyncApi.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                bool isValidUser = false;
                using (var connection = DbConnectionFactory.GetConnection(request.SourceSystem))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("SELECT COUNT(1) FROM Users WHERE user_loginname = @UserName AND user_password = @Password", connection);
                    command.Parameters.AddWithValue("@UserName", request.Username);
                    command.Parameters.AddWithValue("@Password", request.Password);
                    var result = (int)await command.ExecuteScalarAsync();
                    isValidUser = result > 0;
                }
                if (isValidUser)
                {
                    using (var connection = DbConnectionFactory.GetConnection(request.SourceSystem))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("UPDATE Users SET user_loggedin = 1, lastlogin = GETDATE() WHERE user_loginname = @UserName", connection))
                        {
                            command.Parameters.AddWithValue("@UserName", request.Username);
                            int rows = command.ExecuteNonQuery();
                        }
                    }
                    return Content(HttpStatusCode.OK, new { Message = Common.Constants.Messages.USER_AUTHENTICATION_SUCCESSFUL });
                }
                else
                {
                    using (var connection = DbConnectionFactory.GetConnection(request.SourceSystem))
                    {
                        connection.Open();
                        using (var command = new SqlCommand("UPDATE Users SET fattempt = fattempt + 1 WHERE user_loginname = @UserName", connection))
                        {
                            command.Parameters.AddWithValue("@UserName", request.Username);
                            int rows = command.ExecuteNonQuery();
                        }
                    }
                    return Content(HttpStatusCode.Unauthorized, new { Message = Common.Constants.Messages.INVALID_USERNAME_OR_PASSWORD, Error = Common.Constants.Errors.ERR_LOGIN_FAILED });

                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error in Login: {ex.Message}");
                Logger.Log($"StackTrace in Login: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, Common.Constants.Messages.AN_UNEXPECTED_ERROR_OCCURRED);
                throw;
            }
        }
    }
}