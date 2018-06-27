namespace BugTrack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TicketNotifications", "RecipientID", c => c.String(maxLength: 128));
            CreateIndex("dbo.TicketNotifications", "RecipientID");
            AddForeignKey("dbo.TicketNotifications", "RecipientID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TicketNotifications", "RecipientID", "dbo.AspNetUsers");
            DropIndex("dbo.TicketNotifications", new[] { "RecipientID" });
            AlterColumn("dbo.TicketNotifications", "RecipientID", c => c.String());
        }
    }
}
