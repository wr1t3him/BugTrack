using BugTrack.Assist;
using BugTrack.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BugTrack.ActionFilter
{
    public class AttachmentAuthorize : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var attachmentId = filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value;

            if (userId == null || (userId != null && userId != db.TicketAttachments.Find(attachmentId).UserID))
            {
                var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                var msg = "You are a " + myRole + " trying to Edit an Attachment that you did not write.";
                filterContext.Controller.TempData.Add("unauthorizedmsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}