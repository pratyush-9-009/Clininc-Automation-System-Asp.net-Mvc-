using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Finalproject.App_Start;
using System.Web.Security;
namespace Finalproject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var athcookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (athcookie != null)
            {
                FormsAuthenticationTicket authticket = FormsAuthentication.Decrypt(athcookie.Value);
                if (authticket != null && !authticket.Expired)
                {
                    var roles = authticket.UserData.Split(',');
                    HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authticket), roles);
                }
            }
        }
    }
}
