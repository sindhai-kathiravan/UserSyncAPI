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
            string method = actionContext.Request.Method.ToString();

            var model = actionContext.ActionArguments.Values.FirstOrDefault();

            if (actionContext.Request.Headers.Contains(Common.Constants.Headers.CORRELATION_ID))
            {
                correlationId = actionContext.Request.Headers.GetValues(Common.Constants.Headers.CORRELATION_ID).FirstOrDefault();
            }
            if (!actionContext.ActionDescriptor.ActionName.Equals(Common.Constants.ActionNames.GetAllUser, StringComparison.OrdinalIgnoreCase))
            {
                if (model == null)
                {
                    var response = new ApiResponse<object>
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = Common.Constants.Messages.REQUEST_BODY_IS_NULL,
                        Error = Common.Constants.Errors.ERR_NULL_BODY,
                        Data = null,
                        Success = false,
                        CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                    };
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                    PrintResponse(actionContext.Response);
                    return;
                }
            }

            if (!actionContext.ModelState.IsValid)
            {
                HttpResponseMessage responseMessage = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                var response = new ApiResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = Common.Constants.Messages.THE_REQUEST_IS_INVALID,
                    Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
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
                var deleteUserRequest = model as DeleteUserRequest;

                if (createUserRequest != null || updateUserRequest != null || deleteUserRequest != null)
                {
                    if (((createUserRequest != null) && ((createUserRequest.TargetDatabases == null) || (!createUserRequest.TargetDatabases.Any())))
                        || ((updateUserRequest != null) && ((updateUserRequest.TargetDatabases == null || !updateUserRequest.TargetDatabases.Any())))
                        || ((deleteUserRequest != null) && ((deleteUserRequest.TargetDatabases == null || !deleteUserRequest.TargetDatabases.Any()))))
                    {
                        var response = new ApiResponse<object>
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = Common.Constants.Messages.AT_LEAST_ONE_TARGET_DATABASE_MUST_BE_SPECIFIED,
                            Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
                            Data = null,
                            Success = false,
                            CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                        };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        PrintResponse(actionContext.Response);
                        return;
                    }
                    string sourceSystem = createUserRequest?.SourceSystem
                                       ?? updateUserRequest?.SourceSystem
                                       ?? deleteUserRequest?.SourceSystem;

                    var sourceSystemKey = ConfigurationManager.AppSettings[sourceSystem];
                    if (sourceSystemKey == null)
                    {
                        var response = new ApiResponse<object>
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = Common.Constants.Messages.INVALID_SOURCE_SYSTEM,
                            Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
                            Data = string.Format(Common.Constants.Messages.THE_SOURCE_SYSTEM_XXXX_DOES_NOT_EXIST_IN_THE_SYSTEM_LIST, sourceSystem),
                            Success = false,
                            CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                        };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        PrintResponse(actionContext.Response);
                        return;
                    }

                    List<string> targetDatabases = createUserRequest?.TargetDatabases
                                                ?? updateUserRequest?.TargetDatabases
                                                ?? deleteUserRequest?.TargetDatabases;

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
                            Message = Common.Constants.Messages.INVALID_DATABASE_KEY_FOUND,
                            Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
                            Data = JToken.Parse(json),
                            Success = false,
                            CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                        };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        PrintResponse(actionContext.Response);
                        return;
                    }

                    if (updateUserRequest != null || deleteUserRequest != null)
                    {
                        int? userId = updateUserRequest?.UserId
                                  ?? deleteUserRequest?.UserId;
                        if (userId <= 0)
                        {
                            var response = new ApiResponse<object>
                            {
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Status = HttpStatusCode.BadRequest.ToString(),
                                Message = Common.Constants.Messages.INVALID_USER_ID,
                                Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
                                Data = string.Format(Common.Constants.Messages.THE_USER_ID_XX_IS_INVALID, updateUserRequest.UserId),
                                Success = false,
                                CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                            };
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                            PrintResponse(actionContext.Response);
                            return;
                        }

                        var missingDbs = CheckUserExistsInAllDatabases(userId, targetDatabases);
                        if (missingDbs.Any())
                        {
                            var errorObj = new
                            {
                                Errors = missingDbs.Select(db => $"The user id {userId} does not exist in the database '{db}'.")
                            };
                            string json = JsonConvert.SerializeObject(errorObj);

                            var response = new ApiResponse<object>
                            {
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                Status = HttpStatusCode.BadRequest.ToString(),
                                Message = Common.Constants.Messages.USER_ID_DOES_NOT_EXIST,
                                Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
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
                else
                {
                    if ((method == Common.Constants.Methods.GET) || (method == Common.Constants.Methods.DELETE))
                    {
                        if (!actionContext.ActionDescriptor.ActionName.Equals(Common.Constants.ActionNames.GetAllUser, StringComparison.OrdinalIgnoreCase))
                        {
                            var queryParams = actionContext.Request.GetQueryNameValuePairs();
                            var userIdParam = queryParams.FirstOrDefault(q => q.Key.Equals(Common.Constants.QueryStrings.UserId, StringComparison.OrdinalIgnoreCase));
                            if (string.IsNullOrEmpty(userIdParam.Value))
                            {
                                var response = new ApiResponse<object>
                                {
                                    StatusCode = (int)HttpStatusCode.BadRequest,
                                    Status = HttpStatusCode.BadRequest.ToString(),
                                    Message = Common.Constants.Messages.USERID_QUERY_PARAMETER_IS_REQUIRED,
                                    Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
                                    Data = Common.Constants.Messages.USERID_QUERY_PARAMETER_IS_REQUIRED,
                                    Success = false,
                                    CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                                };
                                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                                PrintResponse(actionContext.Response);
                                return;
                            }
                        }
                    }
                }
                if (actionContext.ActionDescriptor.ActionName.Equals(Common.Constants.ActionNames.Login, StringComparison.OrdinalIgnoreCase))
                {
                    var loginRequest = model as LoginRequest;

                    var sourceSystemKey = ConfigurationManager.AppSettings[loginRequest.SourceSystem];
                    if (sourceSystemKey == null)
                    {
                        var response = new ApiResponse<object>
                        {
                            StatusCode = (int)HttpStatusCode.BadRequest,
                            Status = HttpStatusCode.BadRequest.ToString(),
                            Message = Common.Constants.Messages.INVALID_SOURCE_SYSTEM,
                            Error = Common.Constants.Errors.ERR_VALIDATION_FAILUED,
                            Data = string.Format(Common.Constants.Messages.THE_SOURCE_SYSTEM_XXXX_DOES_NOT_EXIST_IN_THE_SYSTEM_LIST, loginRequest.SourceSystem),
                            Success = false,
                            CorrelationId = string.IsNullOrEmpty(correlationId) ? Guid.NewGuid() : Guid.Parse(correlationId),
                        };
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, response);
                        PrintResponse(actionContext.Response);
                        return;
                    }
                }
                Logger.Log("Validation succeeded.");
                return;
            }
        }
        private List<string> CheckUserExistsInAllDatabases(int? userId, List<string> targetDatabases)
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