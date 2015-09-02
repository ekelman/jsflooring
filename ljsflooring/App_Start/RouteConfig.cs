using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ljsflooring
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Listing",
                "Home/GetCategoryListings/{categoryid}/{categoryname}/{page}",
                new { controller = "Home", action = "GetCategoryListings", page = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    "ListingHome",
            //    "Home/GetCategoryListings",
            //    new { controller = "Home", action = "Index" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
            //routes.MapMvcAttributeRoutes(); //Enables Attribute Routing
        }
    }
}
