using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CamelCase.CustomResult
{
    public class JsonCamelCaseResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType) ? ContentType : "application/json";
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                response.Write(JsonConvert.SerializeObject(Data, jsonSerializerSettings));
            }
        }
    }

    public static class JsonCamelCaseHelper
    {
        public static JsonCamelCaseResult JsonCamelCase(this Controller controller, object data)
        {
            return new JsonCamelCaseResult
            {
                Data = data
            };
        }
    }
}