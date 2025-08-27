using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                correlationId = actionContext.Request.Headers
                               .GetValues(Common.Common.Headers.CORRELATION_ID)
                               .FirstOrDefault();
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
                var createUserRequest = model as CreateUserRequest;  // 👈 safe cast

                if (createUserRequest != null) 
                {
                    if (createUserRequest.TargetDatabases == null || !createUserRequest.TargetDatabases.Any())
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
                        return;
                    }

                    var validKeys = ConfigurationManager.AppSettings.AllKeys; 
                    var invalidKeys = createUserRequest.TargetDatabases
                                            .Where(k => !validKeys.Contains(k))
                                            .ToList();

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
                        return;
                    }
                }
                //if (model.TargetDatabases == null || !model.TargetDatabases.Any())
                //{
                //    errors.Add("At least one target database must be specified.");
                //}
                Logger.Log("Validation succeeded.");
                return;
            }
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