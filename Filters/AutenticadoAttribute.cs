using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace imnobiliatiaNet.Filters
{
    public class AutenticadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var rol = session.GetString("UsuarioRol");

            if (string.IsNullOrEmpty(rol))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
