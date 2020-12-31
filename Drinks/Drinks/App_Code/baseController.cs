using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Drinks.App_Code
{
    public abstract class BaseController : Controller
    {
        protected override JsonResult Json(object data, string contentType
                , System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonDotNetJsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }


    }

    public class JsonDotNetJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((this.JsonRequestBehavior == System.Web.Mvc.JsonRequestBehavior.DenyGet)
                    && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("MvcResources.JsonRequest_GetNotAllowed");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                var data = JsonConvert.SerializeObject(this.Data, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                    StringEscapeHandling = StringEscapeHandling.EscapeHtml
                });
                response.Write(data);
            }
        }
    }
}