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

    public class DashboardVM2
    {
        public TicketDashboardData TicketData { get; set; }
        public ProjectDashboardData ProjectData { get; set; }

         public TableDashboardData TableData { get; set; }

        public DashboardVM2()
        {
            TableData = new TableDashboardData();
            TicketData = new TicketDashboardData();
            ProjectData = new ProjectDashboardData();
        }
    }

    public class ProjectDashboardData
    {
        public int ProjectCnt { get; set; }
        public int OpenProjects { get; set; }
        public int Closed { get; set; }
        public int OnHoldProjects { get; set; }
        public int WaitingForApprovalProjects { get; set; }
    }

    public class TicketDashboardData
    {
        public int TicketCnt { get; set; }
        public int UnAssignedTicketCnt { get; set; }
        public int InProgressTicketCnt { get; set; }
        public int OnHoldTicketCnt { get; set; }
        public int ResolvedTicketCnt { get; set; }
        public int ClosedTicketCnt { get; set; }
        public int TicketNotificationCnt { get; set; }
        public int TicketAttachmentCnt { get; set; }
        public int TicketCommentCnt { get; set; }
        public int TicketHistoryCnt { get; set; }
    }

    public class TableDashboardData
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<TicketNotification> TicketNotifications { get; set; }
        public List<TicketAttachment> TicketAttachments { get; set; }
        public List<TicketComment> TicketComments { get; set; }
        public List<TicketHistory> TicketHistories { get; set; }
    }
}