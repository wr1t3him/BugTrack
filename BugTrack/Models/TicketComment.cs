using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTrack.Models
{
    public class TicketComment
    {
        public int ID { get; set; }
        [AllowHtml]
        public string Comment { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketID { get; set; }
        public string UserID { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}