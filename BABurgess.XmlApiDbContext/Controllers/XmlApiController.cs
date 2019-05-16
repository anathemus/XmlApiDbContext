﻿using AutoMapper;
using BABurgess.XmlApiDbContext.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Http;
using System.Xml.Serialization;
using BABurgess.XmlApiDbContext.Controllers;
using System.Net.Http;
using System.Net;

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

        // GET <controller>
        public IEnumerable<string> Get()
        {
            Logger.Information("URL: {HttpRequestUrl}");
            return new string[] { "value1", "value2" };
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

        // GET <controller>/?u=username&p=password
        public HttpResponseMessage Get(string u, string p)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AccountModel));

            string jsonResponse = "";
            string filename = u + ".xml";
            string path = @"D:\Source\BABurgess.XmlApiDbContext\BABurgess.XmlApiDbContext\App_Data\"
                        + filename;

            if (File.Exists(path))
            {
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
                jsonResponse = serializer.Deserialize(file).ToString();

                HttpStatusCode code = HttpStatusCode.OK;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.Content = new StringContent(jsonResponse);
                return message;
            }
            else
            {
                HttpStatusCode code = HttpStatusCode.NotFound;
                HttpResponseMessage message = new HttpResponseMessage(code);
                message.ReasonPhrase = "Account with Username not found.";
                return message;
            }
        }

        // POST <controller>
        public HttpResponseMessage Post([FromBody]string value)
        {
            AccountModel model = new AccountModel();
            XmlSerializer serializer = new XmlSerializer(typeof(AccountModel));
            byte[] byteArray = Encoding.ASCII.GetBytes(value);
            MemoryStream stream = new MemoryStream(byteArray);
            model = serializer.Deserialize(stream) as AccountModel;

            string filename = model.UserId + ".xml";
            string path = @"D:\Source\BABurgess.XmlApiDbContext\BABurgess.XmlApiDbContext\App_Data\"
                        + filename;

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
                message.Headers.Location = new Uri(Environment.CurrentDirectory + "?u="
                    + model.UserId + "&p=" +model.PasswordHash);
                return message;
            }
        }

        // PUT <controller>/?u=username&p=password
        public HttpResponseMessage Put(string u, string p, [FromBody]string value)
        {
            AccountModel model = new AccountModel();
            XmlSerializer serializer = new XmlSerializer(typeof(AccountModel));
            byte[] byteArray = Encoding.ASCII.GetBytes(value);
            MemoryStream stream = new MemoryStream(byteArray);
            model = serializer.Deserialize(stream) as AccountModel;

            if (model.UserId == u && model.PasswordHash == p)
            {
                string filename = model.UserId + ".xml";
                string path = @"D:\Source\BABurgess.XmlApiDbContext\BABurgess.XmlApiDbContext\App_Data\"
                            + filename;

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

        // DELETE <controller>/?u=username&p=password
        public HttpResponseMessage Delete(string u, string p)
        {
            string filename = u + ".xml";
            string path = @"D:\Source\BABurgess.XmlApiDbContext\BABurgess.XmlApiDbContext\App_Data\"
                        + filename;

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
    }
}