using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Owin;
using Pos.Services.Web.Filters;

namespace Pos.Services.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration configuration = new HttpConfiguration();
            configuration.MessageHandlers.Add((DelegatingHandler)new DigestAuthenticationHandler());
            configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            configuration.Formatters.Remove((MediaTypeFormatter)configuration.Formatters.XmlFormatter);
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute("Pos.Services.Web", "api/{controller}/{id}", (object)new
            {
                id = RouteParameter.Optional
            });
            appBuilder.Use(configuration);
        }
    }
}