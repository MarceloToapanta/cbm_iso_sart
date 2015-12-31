using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace CBM_SART.ActionFilter
{
    public class UserFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var ActionName = filterContext.ActionDescriptor.ActionName;
            HttpContext ctx = HttpContext.Current;
            if ((ctx.Session["Usuario"] == null) && (ActionName != "GetImage"))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {{ "Controller", "Usuario" },
                                      { "Action", "Ingresar"}, {"url" , filterContext.HttpContext.Request.Url } });
            }
            else if (ctx.Session["Usuario"] != null)
            {
                CBM_SART.Models.iso_usuario usuario_actual = (CBM_SART.Models.iso_usuario)ctx.Session["Usuario"];
                
                if ((ControllerName == "Empresa" && ActionName != "GetImage" && usuario_actual.ius_tipo_usuario != "Administrador")
                 || (ControllerName == "Departamento" && usuario_actual.ius_tipo_usuario != "Administrador")
                 || (ControllerName == "Usuario" && ActionName != "Ingresar" && ActionName != "UsuarioActual"  && usuario_actual.ius_tipo_usuario != "Administrador")
                 || (ControllerName == "PermisoVigilacia" && usuario_actual.ius_tipo_usuario != "Administrador")
                 || (ControllerName == "Configuracion" && usuario_actual.ius_tipo_usuario != "Administrador"))
                {
                    ctx.Session["ErroUser"] = "Acceso denegado, Usted no está autorizado para visitar esta página";
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {{ "Controller", "Home" },
                                      { "Action", "Index"}});
                }
                else if ((ControllerName == "HistoriaClinica" && usuario_actual.ius_tipo_usuario == "Normal"))
                {
                    ctx.Session["ErroUser"] = "Acceso denegado, Usted no está autorizado para visitar los modulos de Histórias Clinicas";
                    if (ActionName != "Panel") { 
                        filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {{ "Controller", "HistoriaClinica" },
                                              { "Action", "Panel"}});
                    }
                }
                
            }

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
        }


        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
            Debug.WriteLine(message, "Action Filter Log");
        }




    }
}