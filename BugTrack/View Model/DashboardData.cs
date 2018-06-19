using BugTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BugTrack.View_Model
{
    public class DashboardData
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TicketComment> TicketComments { get; set; }
        public List<TicketAttachment> TicketAttachments { get; set; }

        public DashboardData()
        {
            this.Projects = new List<Project>();
            this.Tickets = new List<Ticket>();
            this.TicketComments = new List<TicketComment>();
            this.TicketAttachments = new List<TicketAttachment>();
        }
    }
}