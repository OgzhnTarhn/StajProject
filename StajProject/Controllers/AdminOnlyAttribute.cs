using System.Web.Mvc;
using System.Web;

namespace StajProject.Controllers
{
    public class AdminOnlyAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Session'dan rol bilgisini al
            var role = httpContext.Session["Role"];

            // Admin (A) rolüne sahip mi kontrol et
            return role != null && role.ToString() == "A";
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Admin değilse Trips sayfasına yönlendir
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary(
                    new { controller = "Block", action = "Index" }
                )
            );
        }
    }
}
