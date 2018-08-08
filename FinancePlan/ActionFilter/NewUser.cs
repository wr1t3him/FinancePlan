using FinancePlan.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancePlan.ActionFilter
{
    public class NewUserAuthorize : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var invite = db.Invitations.FirstOrDefault(i => i.Email);
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(db));
            //var user = userManager.FindByEmail(acceptInviteVM.Email);

            //if (user == null)
            //{
            //    RegisterViewModel register;
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}