using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using UserSyncApi.Authentication;
using UserSyncApi.Common;
using UserSyncApi.Helpers;
using UserSyncApi.Models;

namespace UserSyncApi.Controllers
{
    [BasicAuthentication]
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("api/users/create")]
        public IHttpActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                //var response = new ApiResponse<object>
                //{
                //    StatusCode = 200,
                //    Message = "User Created successfully",
                //    Data = ""
                //};
                // throw new Exception("test");
                return Content(HttpStatusCode.OK, "User Created successfully.");

            }
            catch (Exception ex)
            {
                Logger.Log($"Error in CreateUser: {ex.Message}");
                Logger.Log($"StackTrace in CreateUser: { ex.StackTrace.ToString()}");

                //var errorResponse = new ApiResponse<string>
                //{
                //    StatusCode = 500,
                //    Message = "An unexpected error occurred",
                //    Data = ex.Message
                //};

                return Content(HttpStatusCode.InternalServerError, "An unexpected error occurred.");

            }
        }

        [HttpGet]
        [Route("api/users/get")]
        public IHttpActionResult GetUser(int UserId)
        {
            try
            {
                var users = new List<dynamic>
                        {
                            new { Id = 1, Name = "John Doe", Email = "john@example.com", Age = 30 },
                            new { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Age = 28 }
                        };
                if (UserId <= 0)
                {
                    return BadRequest($"{UserId} is {Common.Common.Messages.INVALID_USER_ID}"); // Filter wraps this
                }

                var user = users.FirstOrDefault(u => u.Id == UserId);

                if (user == null)
                {
                    return Content(HttpStatusCode.NotFound, new { Message = $"User with Id {UserId} was not found." });
                }

                return Ok(user);

            }
            catch (Exception ex)
            {
                Logger.Log($"Error in GetUser: { ex.Message}");
                Logger.Log($"StackTrace in GetUser: { ex.StackTrace.ToString()}");
                return Content(HttpStatusCode.InternalServerError, new { Message = "An unexpected error occurred. Please try again later." });
            }

        }

        [HttpGet]
        [Route("api/users/getAll")]
        public IHttpActionResult GetAllUser()
        {
            // sync logic here
            return Ok("Users synchronized successfully");
        }

        [HttpPut]
        [Route("api/users/update")]
        public IHttpActionResult UpdateUser()
        {
            // sync logic here
            return Ok("Users updated successfully");
        }

        [HttpDelete]
        [Route("api/users/delete")]
        public IHttpActionResult DeleteUser()
        {
            // sync logic here
            return Ok("Users updated successfully");
        }


    }
}