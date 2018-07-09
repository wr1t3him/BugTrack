using BugTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTrack.Assist
{
    public class TicketHelp
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();


        public ICollection<Ticket> GetProjectTickets(string userId)
        {
            var myTickets = new List<Ticket>();
            foreach (var project in projHelper.ListUserProjects(userId))
            {
                myTickets.AddRange(db.Tickets.Where(t => t.ProjectID == project.ID).ToList());
            }
            return myTickets;

        }

        public ICollection<Ticket> GetProjectTicketsUsingLinq(string userId)
        {
            return db.Users.Find(userId).Projects.SelectMany(t => t.Tickets).ToList();
        }

        public bool IsTicketOnMyProjects(string userId, int id)
        {
            return db.Users.AsNoTracking().FirstOrDefault(u => u.Id == userId).Projects.SelectMany(t => t.Tickets).Select(t => t.ID).Contains(id);
        }

        public ICollection<Ticket> GetMyTickets(string userId)
        {
            var mytickets = new List<Ticket>();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    mytickets.AddRange(db.Tickets.ToList());
                    break;
                case "ProjectManager":
                    mytickets.AddRange(GetProjectTickets(userId));
                    break;
                case "Developer":
                    mytickets.AddRange(db.Tickets.Where(t => t.AssignedToUserID == userId).ToList());
                    break;
                case "Submitter":
                    mytickets.AddRange(db.Tickets.Where(t => t.OwnerUserID == userId).ToList());
                    break;
            }
            return mytickets;

        }
    }
}