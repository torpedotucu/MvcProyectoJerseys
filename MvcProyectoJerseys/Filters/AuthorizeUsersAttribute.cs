using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MvcProyectoJerseys.Filters
{
    public class AuthorizeUsersAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            string controller = context.RouteData.Values["controller"].ToString();
            string action = context.RouteData.Values["action"].ToString();
            var id = context.RouteData.Values["id"];
            ITempDataProvider provider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();
            var TempData = provider.LoadTempData(context.HttpContext);
            TempData["controller"]=controller;
            TempData["action"]=action;
            if (id!=null)
            {
                TempData["id"]=id.ToString();
            }
            else
            {
                TempData.Remove("id");
            }
            provider.SaveTempData(context.HttpContext, TempData);
            if (user.Identity.IsAuthenticated==false)
            {
                context.Result=this.GetRoute("Usuarios", "Login");
            }
            //var user = context.HttpContext.User;
            //if (user.Identity.IsAuthenticated == false)
            //{
            //    RouteValueDictionary routeLogin =
            //        new RouteValueDictionary(new
            //        {
            //            controller = "Usuarios",
            //            action = "Login"
            //        });
            //    context.Result = new
            //        RedirectToRouteResult(routeLogin);
            //}

        }


        private RedirectToRouteResult GetRoute(string controller,string accion)
        {
            RouteValueDictionary ruta = new RouteValueDictionary(
                new
                {
                    controller = controller,
                    action = accion
                });
            RedirectToRouteResult result = new RedirectToRouteResult(ruta);
            return result;
        }
    }
}
