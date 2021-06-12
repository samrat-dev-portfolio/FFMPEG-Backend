using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace FFMPEG_Demo
{
    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }
    public class CustomHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // add header to request if you want
            var response = await base.SendAsync(request, cancellationToken);
            if(request.Method == HttpMethod.Options && response.StatusCode == HttpStatusCode.MethodNotAllowed)
            {
                response.Headers.Add("Access-Control-Allow-Header", "*");
                //response.Headers.Add("Access-Control-Allow-Header", "accept,content-type,origin,x-my-header");
                //response.Headers.Add("Access-Control-Allow-Origin", "*");
                response = new HttpResponseMessage(HttpStatusCode.OK);
            }
            return response;
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
               name: "ActionIdFile",
               routeTemplate: "api/{controller}/{action}/{id}/{filename}"
           );
            config.Routes.MapHttpRoute(
               name: "ActionId",
               routeTemplate: "api/{controller}/{action}/{id}"
           );
            config.Routes.MapHttpRoute(
                name: "Action",
                routeTemplate: "api/{controller}/{action}"
            );
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.EnableCors();
            config.MessageHandlers.Add(new CustomHeaderHandler());
            config.Formatters.Add(new CustomJsonFormatter());
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
            //config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}
