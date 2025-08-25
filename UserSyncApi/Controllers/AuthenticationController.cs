using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using UserSyncApi.Models;

namespace UserSyncApi.Controllers
{
    public class AuthenticationController : ApiController
    {
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required.");
            }

            // TODO: Replace with real authentication logic (DB check, AD, etc.)
            if (request.Username == "admin" && request.Password == "password123")
            {
                // Normally you’d generate a JWT or token here
                var token = "mock-jwt-token-12345";

                return Ok(new
                {
                    Message = "Login successful",
                    Token = token
                });
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}