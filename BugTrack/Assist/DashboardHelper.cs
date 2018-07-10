using BugTrack.Models;
using BugTrack.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrack.Assist
{
    public static class DashboardHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static TicketDashboardData LoadTicketDashboardData(this TicketDashboardData ticketDashboardData)
        {
            ticketDashboardData.UnAssignedTicketCnt = db.Tickets.Where(t => t.TicketStatus.Name == "UnAssigned").Count();
            ticketDashboardData.InProgressTicketCnt = db.Tickets.Where(t => t.TicketStatus.Name == "In Progress").Count();
            ticketDashboardData.OnHoldTicketCnt = db.Tickets.Where(t => t.TicketStatus.Name == "On Hold").Count();
            ticketDashboardData.ResolvedTicketCnt = db.Tickets.Where(t => t.TicketStatus.Name == "Resolved").Count();
            ticketDashboardData.ClosedTicketCnt = db.Tickets.Where(t => t.TicketStatus.Name == "Closed").Count();

            return ticketDashboardData;
        }
    }
}