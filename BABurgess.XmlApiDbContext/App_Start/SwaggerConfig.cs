using Swashbuckle.Application;
using System;
using System.IO;
using System.Reflection;
using System.Web.Http;

namespace BABurgess.XmlApiDbContext
{
    public static class SwaggerConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            // Use http://localhost:5000/swagger/ui/index to inspect API docs
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "XmlApiDbContext by Benjamin Burgess");
                    c.PrettyPrint();
                    c.RootUrl(x =>
                    {
                        var idx = x.RequestUri.AbsoluteUri.IndexOf("swagger", StringComparison.InvariantCultureIgnoreCase);
                        return x.RequestUri.AbsoluteUri.Substring(0, idx - 1);
                    });

                    // This code allow you to use XML-comments
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".XML";
                    var commentsFile = Path.Combine(baseDirectory, "bin/" + commentsFileName);

                    c.IncludeXmlComments(commentsFile);
                })
                .EnableSwaggerUi();
        }
    }
}