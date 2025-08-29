using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using UserSyncApi.Common;

namespace UserSyncApi.Authentication
{

    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;
            if (authHeader != null && authHeader.Scheme == "Basic")
            {
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter));
                var parts = credentials.Split(':');
                var username = parts[0];
                var password = parts[1];
                if (username == "admin" && password == "password123")
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
                    Logger.Log("Authentication succeeded.");
                    return;
                }
            }
            // Unauthorized
            Logger.Log("Authorization has been denied.");
            var response = new ApiResponse<object>
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Status = HttpStatusCode.Unauthorized.ToString(),
                Message = Common.Constants.Messages.AUTHENTICATION_FAILED,
                Error = Common.Constants.Errors.ERR_UNAUTHORIZED,
                Data = null,
                Success = false,
                CorrelationId = Guid.NewGuid()
            };
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, response);

            //actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, "Authorization has been denied.");
        }
    }
}