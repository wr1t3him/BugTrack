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
    public class TicketAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ticketId = filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value;
            var ticket = db.Tickets.Find(ticketId);
            string userId = HttpContext.Current.User.Identity.GetUserId();

            if (ticket == null || userId == null)
            {
                filterContext.Controller.TempData.Add("unauthorizedmsg", "Contact a supervisor for accessed entry.");
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }
            else if (userId != null && ticket != null)
            {
                var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                if ((myRole == "Developer" && ticket.AssignedToUserID != userId) || (myRole == "Submitter" && ticket.OwnerUserID != userId))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                    var phrase = myRole == "Developer" ? "are not assigned to" : "do not own";
                    var msg = "You are a " + myRole + " trying to Edit a Ticket that you " + phrase;
                    filterContext.Controller.TempData.Add("unauthorizedmsg", msg);
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}