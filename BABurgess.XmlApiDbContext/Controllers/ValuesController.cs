﻿using AutoMapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Web.Http;

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
    public class ValuesController : ApiController
    {
        private ILogger Logger { get; }
        private IMapper Mapper { get; }

        public ValuesController(ILogger logger, IMapper mapper)
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

        // GET <controller>/5
#pragma warning disable RECS0154 // Parameter is never used
        public string Get(int id)
#pragma warning restore RECS0154 // Parameter is never used
        {
            return "value";
        }

        // POST <controller>
#pragma warning disable RECS0154 // Parameter is never used
        public void Post([FromBody]string value)
#pragma warning restore RECS0154 // Parameter is never used
        {
        }

        // PUT <controller>/5
#pragma warning disable RECS0154 // Parameter is never used
#pragma warning disable RECS0154 // Parameter is never used
        public void Put(int id, [FromBody]string value)
#pragma warning restore RECS0154 // Parameter is never used
#pragma warning restore RECS0154 // Parameter is never used
        {
        }

        // DELETE <controller>/5
#pragma warning disable RECS0154 // Parameter is never used
        public void Delete(int id)
#pragma warning restore RECS0154 // Parameter is never used
        {
        }
    }
}