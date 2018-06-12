using BugTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTrack.Assist
{
    public class ProjectsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string UserID, int projectID)
        {
            var project = db.Projects.Find(projectID);
            var Flag = project.User.Any(u => u.Id == UserID);
            return (Flag);
        }

        public ICollection<Project> ListUserProjects(string UserID)
        {
            ApplicationUser user = db.Users.Find(UserID);

            var projects = user.Projects.ToList();
            return (projects);
        }

        public void AddUserToProject(string UserID , int ProjectID)
        {
            if(!IsUserOnProject(UserID, ProjectID))
            {
                Project proj = db.Projects.Find(ProjectID);
                var newUser = db.Users.Find(UserID);

                proj.User.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string UserID, int ProjectID)
        {
            if(IsUserOnProject(UserID, ProjectID))
            {
                Project proj = db.Projects.Find(ProjectID);
                var delUser = db.Users.Find(UserID);

                proj.User.Remove(delUser);
                db.Entry(proj).State = EntityState.Modified; //saves this Object instance
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UsersOnProject(int ProjectID)
        {
            return db.Projects.Find(ProjectID).User;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int ProjectID)
        {
            return db.Users.Where(u => u.Projects.All(p => p.ID != ProjectID)).ToList();
        }
    }
}