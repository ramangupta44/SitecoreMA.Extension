using Sitecore.Pipelines;
using System.Web.Http;
using System.Web.Routing;

namespace MA.CustomMarketingApps.Model
{
    public class RegisterHttpRoutes
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configure(Configure);
        }

        protected void Configure(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute("CustomContacts", "sitecore/api/customlists/{listId}/contacts", new
            {
                controller = "CustomContacts",
                action = "customaction", // Name of the action in ActionName attribute         
            });
        }
        //public void Process(PipelineArgs args)
        //{
        //    RegisterRoute(RouteTable.Routes);
        //}
        //protected void RegisterRoute(RouteCollection routes)
        //{
        //    RouteTable.Routes.MapHttpRoute("CustomContacts", "sitecore/api/customlists/{listId}/contacts", new
        //    {
        //        controller = "CustomContacts",
        //        action = "customaction"
        //    });
        //}
    }
}
