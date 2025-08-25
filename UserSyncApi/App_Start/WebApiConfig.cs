using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using UserSyncApi.Authentication;
using UserSyncApi.Filter;
using UserSyncApi.Handler;

namespace UserSyncApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
         {
            // Web API configuration and services
            config.MessageHandlers.Add(new LoggingHandler());
            config.Filters.Add(new ValidateModelAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new ApiResponseFilter());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
