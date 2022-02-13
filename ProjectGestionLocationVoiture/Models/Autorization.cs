using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectGestionLocationVoiture.Models
{
    public class Autorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                // Don't check for authorization as AllowAnonymous filter is applied to the action or controller
                return;
            }

            // Check for authorization
            if ((string)HttpContext.Current.Session["Typeuser"] != "admin")
            {
                filterContext.Result = new RedirectResult("~/Auth/Login"); 
                //filterContext.Result = new RedirectResult("Voitures");
            }
        }
    }
}
