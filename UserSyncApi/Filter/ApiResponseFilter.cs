using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using UserSyncApi.Common;
using UserSyncApi.Models;

namespace UserSyncApi.Filter
{
    public class ApiResponseFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var correlationId = context.Request.Headers.Contains(Common.Constants.Headers.CORRELATION_ID) ? Guid.Parse(context.Request.Headers.GetValues(Common.Constants.Headers.CORRELATION_ID).FirstOrDefault()) : Guid.NewGuid();
            ApiResponse<object> apiResponse = new ApiResponse<object>();

            if (context.Response != null)
            {
                var statusCode = context.Response.StatusCode;

                object content = null;
                if (context.Response.Content != null)
                {
                    context.Response.TryGetContentValue(out content);
                }

                string message = null;
                string error = null;

                if (statusCode == HttpStatusCode.BadRequest)
                {
                    var httpError = content as HttpError;
                    message = httpError != null ? httpError.Message : Common.Constants.Messages.THE_REQUEST_IS_INVALID;
                    error = Common.Constants.Errors.ERR_BAD_REQUEST;
                }
                else if (statusCode == HttpStatusCode.NotFound)
                {
                    string notFoundMessage = null;

                    var httpError = content as HttpError;
                    if (httpError != null && !string.IsNullOrEmpty(httpError.Message))
                    {
                        notFoundMessage = httpError.Message;
                    }
                    else if (content != null)
                    {
                        // Reflection to check if anonymous object has "Message" property
                        var msgProp = content.GetType().GetProperty("Message");
                        if (msgProp != null)
                        {
                            var val = msgProp.GetValue(content, null);
                            if (val != null)
                            {
                                notFoundMessage = val.ToString();
                            }
                        }
                    }

                    message = !string.IsNullOrEmpty(notFoundMessage) ? notFoundMessage : Common.Constants.Messages.THE_REQUESTED_USER_WAS_NOT_FOUND;
                    error = Common.Constants.Errors.ERR_NOT_FOUND;
                }
                else if (statusCode == HttpStatusCode.Unauthorized)
                {
                    var msgProp = content.GetType().GetProperty("Message");
                    if (msgProp != null)
                    {
                        var val = msgProp.GetValue(content, null);
                        if (val != null)
                        {
                            message = val.ToString();
                        }
                    }
                    var msgErr = content.GetType().GetProperty("Error");
                    if (msgErr != null)
                    {
                        var val = msgErr.GetValue(content, null);
                        if (val != null)
                        {
                            error = val.ToString();
                        }
                    }
                    content = null;
                }
                else
                {
                    var msgProp = content.GetType().GetProperty("Message");
                    if (msgProp != null)
                    {
                        var val = msgProp.GetValue(content, null);
                        if (val != null)
                        {
                            message = val.ToString();
                        }
                    }
                    var dataProp = content.GetType().GetProperty("Data");
                    if (dataProp != null)
                    {
                        var val = dataProp.GetValue(content, null);
                        if (val != null)
                        {
                            content = val;//.ToString();
                        }
                    }
                    else {
                        content = null;
                    }
                    //message = statusCode == HttpStatusCode.OK ? Common.Common.Messages.REQUEST_COMPLETED_SUCCESSFULLY : statusCode.ToString();
                }

                apiResponse = new ApiResponse<object>
                {
                    StatusCode = (int)statusCode,
                    Status = statusCode.ToString(),   
                    Message = message,                     
                    Error = error,
                    Data = content,
                    Success = statusCode == HttpStatusCode.OK,
                    CorrelationId = correlationId
                };
                context.Response = context.Request.CreateResponse(statusCode, apiResponse);
            }
            else if (context.Exception != null)
            {
                apiResponse = new ApiResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Status = HttpStatusCode.InternalServerError.ToString(),
                    Message = context.Exception.Message,
                    Error = Common.Constants.Errors.ERR_INTERNAL_SERVER,
                    Data = null,
                    Success = false,
                    CorrelationId = correlationId
                };

                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, apiResponse);
            }
            Logger.Log($"Response: {JToken.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(apiResponse))}");
            base.OnActionExecuted(context);
        }
    }

}