using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UserSyncApi.Common;

namespace UserSyncApi.Authentication
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string correlationId = null;
            if (actionContext.Request.Headers.Contains(Common.Common.Headers.CORRELATION_ID))
            {
                correlationId = actionContext.Request.Headers
                               .GetValues(Common.Common.Headers.CORRELATION_ID)
                               .FirstOrDefault();
            }

            if (actionContext.ActionArguments.Values.Any(v => v == null))
            {
                var response = new ApiResponse<object>
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = Common.Common.Messages.REQUEST_BODY_IS_NULL,
                    Error=Common.Common.Errors.ERR_NULL_BODY,
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
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
                PrintResponse(actionContext.Response);
                return;
            }
            else
            {
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