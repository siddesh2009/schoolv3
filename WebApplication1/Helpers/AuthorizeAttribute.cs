using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using School.Business.Contract;
using School.ViewModel;
using SchoolModel.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.WebLayer
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = (schooluser)context.HttpContext.Items["User"];

            //var req = context.HttpContext.Request;
            //req.EnableBuffering();

            //// read the body here as a workarond for the JSON parser disposing the stream
            //if (req.Body.CanSeek)
            //{
            //    req.Body.Seek(0, SeekOrigin.Begin);

            //    // if body (stream) can seek, we can read the body to a string for logging purposes
            //    using (var reader = new StreamReader(
            //         req.Body,
            //         encoding: Encoding.UTF8,
            //         detectEncodingFromByteOrderMarks: false,
            //         bufferSize: 8192,
            //         leaveOpen: true))
            //    {
            //        var jsonString = reader.ReadToEnd();
            //        //var json = JObject.Parse(jsonString);
            //        // store into the HTTP context Items["request_body"]
            //        context.HttpContext.Items.Add("request_body", jsonString);
            //    }

            //    using (var streamReader = new HttpRequestStreamReader(req.Body, Encoding.UTF8)) { 
            //        using (var jsonReader = new JsonTextReader(streamReader))
            //        {
            //            var json = JObject.LoadAsync(jsonReader);
            //            // process JSON
            //        }
            //    }

            //    // go back to beginning so json reader get's the whole thing
            //    req.Body.Seek(0, SeekOrigin.Begin);
            //}

            if (account == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
