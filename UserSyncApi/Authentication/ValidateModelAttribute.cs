using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UserSyncApi.Common;
using UserSyncApi.Models;

namespace UserSyncApi.Authentication
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string correlationId = null;
            var model = actionContext.ActionArguments.Values.FirstOrDefault();

            if (actionContext.Request.Headers.Contains(Common.Common.Headers.CORRELATION_ID))
            {
                correlationId = actionContext.Request.Headers.GetValues(Common.Common.Headers.CORRELATION_ID).FirstOrDefault();
            }

            if (model == null)
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = Common.Common.Messages.REQUEST_BODY_IS_NULL,
                    Error = Common.Common.Errors.ERR_NULL_BODY,
                    Data = null,
                    Success = false,
                    CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                PrintResponse(actionContext.Response);
                return;
            }

            if (!actionContext.ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                var response = new ApiResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = Common.Common.Messages.THE_REQUEST_IS_INVALID,
                    Error = Common.Common.Errors.ERR_VALIDATION_FAILUED,
                    Data = JToken.Parse(responseMessage.Content.ReadAsStringAsync().Result),
                    Success = false,
                    CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                };

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                PrintResponse(actionContext.Response);
                return;
            }
            else
            {
                var createUserRequest = model as CreateUserRequest;
                var updateUserRequest = model as UpdateUserRequest;

               
                if (createUserRequest != null || updateUserRequest != null)
                {
                    if (((createUserRequest != null ) && ((createUserRequest.TargetDatabases == null) || (!createUserRequest.TargetDatabases.Any()))) || ((updateUserRequest!=null) &&((updateUserRequest.TargetDatabases == null || !updateUserRequest.TargetDatabases.Any()))))
                    {
                        var response = new ApiResponse<object>
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = Common.Common.Messages.AT_LEAST_ONE_TARGET_DATABASE_MUST_BE_SPECIFIED,
                            Error = Common.Common.Errors.ERR_VALIDATION_FAILUED,
                            Data = null,
                            Success = false,
                            CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                        };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        PrintResponse(actionContext.Response);
                        return;
                    }
                    string sourceSystem = createUserRequest?.SourceSystem ?? updateUserRequest?.SourceSystem;
                    var sourceSystemKey = ConfigurationManager.AppSettings[sourceSystem];
                    if (sourceSystemKey == null)
                    {
                        var response = new ApiResponse<object>
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = Common.Common.Messages.INVALID_SOURCE_SYSTEM,
                            Error = Common.Common.Errors.ERR_VALIDATION_FAILUED,
                            Data = string.Format(Common.Common.Messages.THE_SOURCE_SYSTEM_XXXX_DOES_NOT_EXIST_IN_THE_SYSTEM_LIST, sourceSystem),
                            Success = false,
                            CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                        };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        PrintResponse(actionContext.Response);
                        return;
                    }

                    List<string> targetDatabases = createUserRequest?.TargetDatabases ?? updateUserRequest?.TargetDatabases;

                    string[] validKeys = ConfigurationManager.AppSettings.AllKeys;
                    var invalidKeys = targetDatabases.Where(k => !validKeys.Contains(k)).ToList();

                    if (invalidKeys.Any())
                    {
                        var errorObj = new
                        {
                            Errors = invalidKeys.Select(k => $"Invalid database key: {k}").ToList()
                        };
                        string json = JsonConvert.SerializeObject(errorObj);

                        var response = new ApiResponse<object>
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = Common.Common.Messages.INVALID_DATABASE_KEY_FOUND,
                            Error = Common.Common.Errors.ERR_VALIDATION_FAILUED,
                            Data = JToken.Parse(json),
                            Success = false,
                            CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                        };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        PrintResponse(actionContext.Response);
                        return;
                    }

                    if (updateUserRequest != null)
                    {
                        if (updateUserRequest.UserId <= 0)
                        {
                            var response = new ApiResponse<object>
                            {
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Status = HttpStatusCode.BadRequest.ToString(),
                                Message = Common.Common.Messages.INVALID_USER_ID,
                                Error = Common.Common.Errors.ERR_VALIDATION_FAILUED,
                                Data = string.Format(Common.Common.Messages.THE_USER_ID_XX_IS_INVALID, updateUserRequest.UserId),
                                Success = false,
                                CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                            };
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                            PrintResponse(actionContext.Response);
                            return;
                        }

                        // Validate if UserId exists in the DB
                        var missingDbs = CheckUserExistsInAllDatabases(updateUserRequest.UserId, updateUserRequest.TargetDatabases);
                        if (missingDbs.Any())
                        {
                            var errorObj = new
                            {
                                Errors = missingDbs.Select(db => $"The user id {updateUserRequest.UserId} does not exist in the database '{db}'.")
                            };
                            string json = JsonConvert.SerializeObject(errorObj);

                            var response = new ApiResponse<object>
                            {
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Status = HttpStatusCode.BadRequest.ToString(),
                                Message = Common.Common.Messages.USER_ID_DOES_NOT_EXIST,
                                Error = Common.Common.Errors.ERR_VALIDATION_FAILUED,
                                Data = JToken.Parse(json),
                                Success = false,
                                CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                            };
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                            PrintResponse(actionContext.Response);
                            return;
                        }
                    }
                }
                Logger.Log("Validation succeeded.");
                return;
            }
        }
        private List<string> CheckUserExistsInAllDatabases(int userId, List<string> targetDatabases)
        {
            var missingDbKeys = new List<string>();

            foreach (var dbKey in targetDatabases)
            {
                string connString = ConfigurationManager.AppSettings[dbKey];
                if (string.IsNullOrEmpty(connString))
                {
                    missingDbKeys.Add(dbKey); // invalid config itself
                    continue;
                }

                using (var conn = new SqlConnection(connString))
                using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE user_id = @UserId", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();

                    if (count == 0)
                    {
                        missingDbKeys.Add(dbKey); // User not found in this DB
                    }
                }
            }

            return missingDbKeys; // Empty list => User exists in all DBs
        }
        private void PrintResponse(HttpResponseMessage response)
        {
            var responseContent = response.Content.ReadAsStringAsync().Result;
            string prettyJson;
            try
            {
                var parsed = JToken.Parse(responseContent);
                prettyJson = parsed.ToString(Formatting.Indented);
            }
            catch
            {
                prettyJson = responseContent;
            }
            Logger.Log("Validation failed.");
            Logger.Log(prettyJson);
        }
    }
}