using BugTrack.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrack.Assist
{
    public class Mytickets
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ProjectsHelper projhelp = new ProjectsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public ICollection<Ticket> ListOfUserTickets(string UserId)
        {
            var userID = UserId;
            var userTickets = new List<Ticket>();
            var myRole = roleHelper.ListUserRoles(userID).FirstOrDefault();

            if(myRole == "Admin")
            {
                userTickets = db.Tickets.Where(t => t.TicketStatus.Name != "Closed").ToList();
            }
            else if (myRole == "Submitter")
            {
                userTickets = db.Tickets.Where(t => t.OwnerUserID == userID).Where(t => t.TicketStatus.Name != "Closed").ToList();
            }
            else if (myRole == "Developer")
            {
                userTickets = db.Tickets.Where(t => t.AssignedToUserID == userID).Where(t => t.TicketStatus.Name != "Closed").ToList();
            }
            else
            {
                var myprojects = projhelp.ListUserProjects(userID);
                foreach (var project in myprojects)
                {
                    var projId = project.ID;
                    userTickets.AddRange(db.Tickets.Where(t => t.ProjectID == projId).Where(t => t.TicketStatus.Name != "Closed").ToList());
                }

            }

            return userTickets.ToList();
        }
    }
}