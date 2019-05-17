﻿using System.Web.Http;

namespace BABurgess.XmlApiDbContext
{
    public static class RouteConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "XmlApi", action = "SwaggerUi",
                    id = RouteParameter.Optional }
            );

        }
    }
}
