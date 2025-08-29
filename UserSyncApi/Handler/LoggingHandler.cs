using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using UserSyncApi.Common;

namespace UserSyncApi.Handler
{
    public class LoggingHandler : DelegatingHandler
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(CorrelationIdHeader))
            {
                request.Headers.Add(CorrelationIdHeader, Guid.NewGuid().ToString());
            }

            // Add to request properties so filters/controllers can access it
            request.Properties[CorrelationIdHeader] = request.Headers.GetValues(CorrelationIdHeader).ToString();

            var ipAddress = HttpContext.Current?.Request?.UserHostAddress;
            var forwardedFor = HttpContext.Current?.Request?.Headers["X-Forwarded-For"];
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                ipAddress = forwardedFor.Split(',')[0];
            }

            var origin = request.Headers.Contains("Origin") ? request.Headers.GetValues("Origin").FirstOrDefault() : null;
            var referer = request.Headers.Referrer?.ToString();
            Logger.Log("===== Incoming Request =====");
            Logger.Log($"[Request Log] IP={ipAddress}, Origin={origin}, Referer={referer}, URL={request.RequestUri}");

            // Log request line
            var method = request.Method.Method;
            var url = request.RequestUri.ToString();

            // Log headers
            var headers = string.Join("\n", request.Headers.Select(h => $"{h.Key}: {string.Join(",", h.Value)}"));

            // Log body (if present)
            string body = string.Empty;
            if (request.Content != null)
            {
                body = await request.Content.ReadAsStringAsync();
            }

            Logger.Log($"Method: {method}");
            Logger.Log($"URL: {url}");
            Logger.Log($"Headers: { headers}");
            if ((method == Common.Constants.Methods.GET) || (method == Common.Constants.Methods.DELETE))
            {
                // Log query string parameters (for GET requests)
                var queryParams = request.GetQueryNameValuePairs();
                if (queryParams.Any())
                {
                    Logger.Log("Query Parameters:");
                    foreach (var kv in queryParams)
                    {
                        Logger.Log($"{kv.Key} = {kv.Value}");
                    }
                }
            }
            Logger.Log($"Body: {body}");

            // Continue processing
            var response = await base.SendAsync(request, cancellationToken);


            // Add the same correlation ID to response headers
            response.Headers.Add(CorrelationIdHeader, request.Headers.GetValues(CorrelationIdHeader));

            return response;
        }
    }
}