using System;
using System.Data.Entity;
using System.ServiceModel.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Devtalk.EF.CodeFirst;
using owaitlist.Controllers;
using owaitlist.Models;

namespace owaitlist
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Banners/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            ModelBinders.Binders.Add(typeof(TimeSpan), new TimeBinder());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new RazorViewEngine());

            Database.SetInitializer<OwlEntities>(new DontDropDbJustCreateTablesIfModelChanged<OwlEntities>());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            SecurityAccessDeniedException se = exception as SecurityAccessDeniedException;
            if (se != null)
            {
                routeData.Values.Add("action", "AccessDenied");
            }
            else
            {
                HttpException httpException = exception as HttpException;

                if (httpException == null)
                {
                    routeData.Values.Add("action", "Error");
                }
                else
                {
                    switch (httpException.GetHttpCode())
                    {
                        case 404:
                            // Page not found.
                            routeData.Values.Add("action", "HttpError404");
                            break;
                        case 500:
                            // Server error.
                            routeData.Values.Add("action", "HttpError500");
                            break;

                        // Here you can handle Views to other error codes.
                        //// I choose a General error template  
                        default:
                            routeData.Values.Add("action", "General");
                            break;
                    }
                }
            }

            // Pass exception details to the target error View.
            routeData.Values.Add("error", exception.Message);

            // Clear the error on server.
            Server.ClearError();

            // Call target Controller and pass the routeData.
            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(
                new HttpContextWrapper(Context), routeData));
        }
    }
}