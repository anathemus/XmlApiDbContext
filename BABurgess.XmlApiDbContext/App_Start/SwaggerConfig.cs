using Swashbuckle.Application;
using Swashbuckle.SwaggerUi;
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
                    c.RootUrl(x =>
                    {
                        var idx = x.RequestUri.AbsoluteUri.IndexOf("swagger", StringComparison.InvariantCultureIgnoreCase);
                        return x.RequestUri.AbsoluteUri.Substring(0, idx - 1);
                    });
                    c.SingleApiVersion("v1", "XmlApiDbContext by Benjamin Burgess")
                    .Description("An ASP.NET Web Api for a financial trading service."
                    + "The information is passed in plaintext XML format. \nIt is then"
                    + "encrypted and stored. \nWhen retrieved, it is decrypted back into"
                    + " plaintext XML and passed back.")
                    .Contact(cc => cc
                    .Name("Benjamin Burgess")
                    .Email("benjamin.a.burgess@outlook.com")
                    .Url("https://benjaminaburgess.azurewebsites.net"))
                    .License(l => l
                    .Name("MIT Licence")
                    .Url("https://opensource.org/licenses/MIT"));
                    c.PrettyPrint();


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