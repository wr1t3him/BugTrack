namespace BugTrack.Migrations
{
    using BugTrack.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTrack.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTrack.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            var RoleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            //var ProjectManager = new RoleManager<IdentityRole>(
            //    new RoleStore<IdentityRole>(context));

            //var Developer = new RoleManager<IdentityRole>(
            //    new RoleStore<IdentityRole>(context));

            //var Submitter = new RoleManager<IdentityRole>(
            //    new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                RoleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                RoleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                RoleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                RoleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));

            //var userProjectManager = new UserManager<ApplicationUser>(
            //    new UserStore<ApplicationUser>(context));

            //Seed a few Demo Users
            if (!context.Users.Any(u => u.Email == "DemoAdmin@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@Mailinator.com",
                    Email = "DemoAdmin@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                }, "AdminBug@1");

                var userID = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
                userManager.AddToRole(userID, "Admin");
            }

            if (!context.Users.Any(u => u.Email == "DemoManager@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoManager@mailinator.com",
                    Email = "DemoManager@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Manager",
                }, "Projectm2");

                var userID = userManager.FindByEmail("DemoManager@mailinator.com").Id;
                userManager.AddToRole(userID, "ProjectManager");
            }

            if (!context.Users.Any(u => u.Email == "demodeveloper@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demodeveloper@mailinator.com",
                    Email = "demodeveloper@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer",
                }, "Develop3!");

                var userID = userManager.FindByEmail("demodeveloper@mailinator.com").Id;
                userManager.AddToRole(userID, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "demosubmitter@mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demosubmitter@mailinator.com",
                    Email = "demosubmitter@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter",
                }, "Submit4!");

                var userID = userManager.FindByEmail("demosubmitter@mailinator.com").Id;
                userManager.AddToRole(userID, "Submitter");
            }

            if (!context.Users.Any(u => u.Email == "wr1t3him@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "wr1t3him@gmail.com",
                    Email = "wr1t3him@gmail.com",
                    FirstName = "Wilten",
                    LastName = "Houston",
                }, "Byakugan301!");

                var userID = userManager.FindByEmail("wr1t3him@gmail.com").Id;
                userManager.AddToRole(userID, "Admin");
            }
            if (!context.Users.Any(u => u.Email == "ColeTrain@maillinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ColeTrain@maillinator.com",
                    Email = "ColeTrain@maillinator.com",
                    FirstName = "Tyrell",
                    LastName = "Coleman",
                }, "Neji301!");

                var userID = userManager.FindByEmail("ColeTrain@maillinator.com").Id;
                userManager.AddToRole(userID, "ProjectManager");
            }

            //var userDeveloper = new UserManager<ApplicationUser>(
            //    new UserStore<ApplicationUser>(context));

            //var userSubmitter = new UserManager<ApplicationUser>(
            //    new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "kenhouston@maillinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "kenhouston@maillinator.com",
                    Email = "kenhouston@maillinator.com",
                    FirstName = "Ken",
                    LastName = "Houston",
                }, "Sharigan301!");

                var userID = userManager.FindByEmail("kenhouston@maillinator.com").Id;
                userManager.AddToRole(userID, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "brittini@maillinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "brittini@maillinator.com",
                    Email = "brittini@maillinator.com",
                    FirstName = "Brittini",
                    LastName = "Houston",
                }, "August@18");

                var userID = userManager.FindByEmail("brittini@maillinator.com").Id;
                userManager.AddToRole(userID, "Submitter");
            }

            if (context.TicketPriorities.Count() == 0)
            {
                context.TicketPriorities.AddOrUpdate(
                    p => p.Name,
                    new TicketPriority { ID = 100, Name = "Immediate" },
                    new TicketPriority { ID = 200, Name = "High" },
                    new TicketPriority { ID = 300, Name = "Medium" },
                    new TicketPriority { ID = 400, Name = "Low" },
                    new TicketPriority { ID = 500, Name = "None" }
                    );
            }

            if (context.TicketStatus.Count() == 0)
            {
                context.TicketStatus.AddOrUpdate(
                p => p.Name,
                new TicketStatus { ID = 100, Name = "UnAssigned" },
                new TicketStatus { ID = 200, Name = "In Progress" },
                new TicketStatus { ID = 300, Name = "On Hold" },
                new TicketStatus { ID = 400, Name = "Resolved" },
                new TicketStatus { ID = 500, Name = "Closed" }
                );
            }

            if (context.TicketTypes.Count() == 0)
            {
                context.TicketTypes.AddOrUpdate(
                p => p.Name,
                new TicketType { ID = 100, Name = "New Bug" },
                new TicketType { ID = 200, Name = "Documentation Needed" },
                new TicketType { ID = 300, Name = "ScreenCast Demo Request" }
                );
            }

            if (context.Projects.Count() == 0)
            {
                context.Projects.AddOrUpdate(
                p => p.Name,
                new Project { ID = 100, Name = "Metal Gear" },
                new Project { ID = 200, Name = "Andromeda" },
                new Project { ID = 300, Name = "Samurai" }
                );
            }

            if (context.Tickets.Count() == 0)
            {
                context.Tickets.AddOrUpdate(
                p => p.Title,
                new Ticket
                {
                    ProjectID = 100,
                    TicketPriorityID = 300,
                    TicketStatusID = 100,
                    TicketTypeID = 100
                });


                context.Tickets.AddOrUpdate(
                    p => p.Title,
                    new Ticket
                    {
                        ProjectID = 200,
                        TicketPriorityID = 200,
                        TicketStatusID = 300,
                        TicketTypeID = 100
                    });

                context.Tickets.AddOrUpdate(
                    p => p.Title,
                    new Ticket
                    {
                        ProjectID = 300,
                        TicketPriorityID = 100,
                        TicketStatusID = 500,
                        TicketTypeID = 100
                    });
            }
        }
    }
}
//AttachDbFilename=BugTrack;
//<add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\aspnet-BugTrack-20180612122306.mdf;Initial Catalog=aspnet-BugTrack-20180612122306;Integrated Security=True" providerName="System.Data.SqlClient" />
