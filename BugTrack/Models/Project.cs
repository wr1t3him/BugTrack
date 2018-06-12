using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrack.Models
{
    public class Project
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }

        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.User = new HashSet<ApplicationUser>();
        }

    }
}