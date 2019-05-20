using AutoMapper;
using BABurgess.XmlApiDbContext.Models;
using Serilog;
using System;
using System.IO;
using System.Text;
using System.Web.Http;
using System.Xml.Serialization;
using System.Net.Http;
using System.Net;
using BABurgess.XmlApiDbContext.Helpers;
using System.Web.Http.Description;

namespace BABurgess.XmlApiDbContext
{
    public class ComplexType
    {
        /// <summary>
        /// This is the object's key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// This is the object's key
        /// </summary>
        public string Value { get; set; }
        public string NullValue { get; set; } = null;
        public int IntValue { get; set; } = 0;
    }

    /// <summary>
    /// Example
    /// </summary>
    public class XmlApiController : ApiController
    {
        private ILogger Logger { get; }
        private IMapper Mapper { get; }

        public XmlApiController(ILogger logger, IMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET - redirects to Swagger Ui
        public HttpResponseMessage Get()
        {
            HttpStatusCode code = HttpStatusCode.Redirect;
            HttpResponseMessage message = new HttpResponseMessage(code);
            message.Headers.Location = new Uri("https://baburgessxmlapidbcontext.azurewebsites.net/swagger/ui/index");
            return message;
        }

        // GET <controller>/?p=passwordhash
        [Route("")]
        [ResponseType(typeof(FinancialAccountUser))]
        public HttpResponseMessage Get([FromBody]string value, string p)
        {
            FinancialAccountUser model = ParseAccount(value);
            if (model.UserName == null || p == null)
            {
                HttpStatusCode code = HttpStatusCode.Redirect;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.Headers.Location = new Uri("https://baburgessxmlapidbcontext.azurewebsites.net/swagger/ui/index");
                return message;
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FinancialAccountUser));

                string path = GetFilePath(p);

                if (File.Exists(path))
                {
                    string jsonResponse = String.Empty;
                    FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                    string encryptedFile = String.Empty;

                    using (StreamReader stream = new StreamReader(file))
                    {
                        encryptedFile = stream.ReadToEnd();
                    }
                    
                    jsonResponse = StringEncryption.DecryptUserAccount(encryptedFile, p);

                    HttpStatusCode code = HttpStatusCode.OK;
                    HttpResponseMessage message = new HttpResponseMessage(code);
                    message.Content = new StringContent(jsonResponse);
                    return message;
                }
                else
                {
                    HttpStatusCode code = HttpStatusCode.NotFound;
                    HttpResponseMessage message = new HttpResponseMessage(code);
                    message.ReasonPhrase = "Account not found.";
                    return message;
                }
            }
        }

        /// <summary>
        /// Getting complex type value
        /// </summary>
        [HttpGet]
        [Route("GetComplex")]
        public ComplexType GetComplex()
        {
            return new ComplexType() { Key = "Key", Value = "Value" };
        }

        [HttpGet]
        [Route("ThrowException")]
        public void ThrowException()
        {
            throw new Exception("Example exception");
        }


        // POST <controller>
        public HttpResponseMessage Post([FromBody]string value, string p)
        {
            FinancialAccountUser model = ParseAccount(value);
            string path = GetFilePath(p);
            XmlSerializer serializer = new XmlSerializer(typeof(FinancialAccountUser));

            if (!File.Exists(path))
            {
                FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                serializer.Serialize(file, model);
                HttpStatusCode code = HttpStatusCode.Created;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.ReasonPhrase = "Account created successfuly";
                return message;
            }
            else
            {
                HttpStatusCode code = HttpStatusCode.Found;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.ReasonPhrase = "Account found! Creation not necessary. Redirecting";
                message.Headers.Location = new Uri(Environment.CurrentDirectory + "?p="
                    + model.PasswordHash);
                return message;
            }
        }

        // PUT <controller>/?u=username&p=password
        public HttpResponseMessage Put([FromBody]string value, string p)
        {
            FinancialAccountUser model = ParseAccount(value);
            string path = GetFilePath(p);
            XmlSerializer serializer = new XmlSerializer(typeof(FinancialAccountUser));
            if (File.Exists(path))
            {
                FileStream file = new FileStream(path, FileMode.Truncate, 
                    FileAccess.ReadWrite);
                serializer.Serialize(file, model);

                HttpStatusCode code = HttpStatusCode.OK;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.ReasonPhrase = "Account successfully updated.";
                return message;
            }
            else
            {
                HttpStatusCode code = HttpStatusCode.PreconditionFailed;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.ReasonPhrase = "Username or Password do not match JSON sent in "
                    + "PUT request.";
                return message;
            }
        }

        // DELETE <controller>/?p=password
        public HttpResponseMessage Delete(string p)
        {
            string path = GetFilePath(p);
            if (File.Exists(path))
            {
                File.Delete(path);
                HttpStatusCode code = HttpStatusCode.OK;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.ReasonPhrase = "Account deleted.";
                return message;
            }
            else
            {
                HttpStatusCode code = HttpStatusCode.NotFound;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.ReasonPhrase = "Account not found.";
                return message;
            }
        }

        [NonAction]
        private FinancialAccountUser ParseAccount(string body)
        {
            FinancialAccountUser model = new FinancialAccountUser();
            XmlSerializer serializer = new XmlSerializer(typeof(FinancialAccountUser));
            byte[] byteArray = Encoding.ASCII.GetBytes(body);
            MemoryStream stream = new MemoryStream(byteArray);
            model = serializer.Deserialize(stream) as FinancialAccountUser;

            return model;
        }

        [NonAction]
        private string GetFilePath(string p)
        {
            string filename = p + ".xml";
            string path = Environment.CurrentDirectory + "/" + filename;
            return path;
        }
    }
}