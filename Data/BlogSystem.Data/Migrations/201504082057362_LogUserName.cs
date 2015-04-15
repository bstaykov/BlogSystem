namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogUserName : DbMigration
    {
        public override void Up()
        {
            this.DropForeignKey("dbo.Logs", "UserId", "dbo.AspNetUsers");
            this.DropIndex("dbo.Logs", new[] { "UserId" });
            this.AddColumn("dbo.Logs", "UserName", c => c.String());
            this.DropColumn("dbo.Logs", "UserId");
        }
        
        public override void Down()
        {
            this.AddColumn("dbo.Logs", "UserId", c => c.String(maxLength: 128));
            this.DropColumn("dbo.Logs", "UserName");
            this.CreateIndex("dbo.Logs", "UserId");
            this.AddForeignKey("dbo.Logs", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
