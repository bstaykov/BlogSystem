namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessagingModel : DbMigration
    {
        public override void Up()
        {
            this.DropForeignKey("dbo.DialogParticipants", "DialogId", "dbo.Dialogs");
            this.DropForeignKey("dbo.DialogParticipants", "UserId", "dbo.AspNetUsers");
            this.DropIndex("dbo.DialogParticipants", new[] { "DialogId" });
            this.DropIndex("dbo.DialogParticipants", new[] { "UserId" });
            this.RenameColumn(table: "dbo.Messages", name: "UserId", newName: "User_Id");
            this.RenameColumn(table: "dbo.Dialogs", name: "StarterId", newName: "User_Id");
            this.RenameIndex(table: "dbo.Messages", name: "IX_UserId", newName: "IX_User_Id");
            this.RenameIndex(table: "dbo.Dialogs", name: "IX_StarterId", newName: "IX_User_Id");
            this.AddColumn("dbo.Messages", "ReadBy", c => c.Int(nullable: false));
            this.AddColumn("dbo.Dialogs", "FirstUserId", c => c.String(maxLength: 128));
            this.AddColumn("dbo.Dialogs", "SecondUserId", c => c.String(maxLength: 128));
            this.CreateIndex("dbo.Dialogs", "FirstUserId");
            this.CreateIndex("dbo.Dialogs", "SecondUserId");
            this.AddForeignKey("dbo.Dialogs", "FirstUserId", "dbo.AspNetUsers", "Id");
            this.AddForeignKey("dbo.Dialogs", "SecondUserId", "dbo.AspNetUsers", "Id");
            this.DropColumn("dbo.Dialogs", "StartedOn");
            this.DropTable("dbo.DialogParticipants");
        }
        
        public override void Down()
        {
            this.CreateTable(
                "dbo.DialogParticipants",
                c => new
                    {
                        DialogId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.DialogId, t.UserId });

            this.AddColumn("dbo.Dialogs", "StartedOn", c => c.DateTime(nullable: false));
            this.DropForeignKey("dbo.Dialogs", "SecondUserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.Dialogs", "FirstUserId", "dbo.AspNetUsers");
            this.DropIndex("dbo.Dialogs", new[] { "SecondUserId" });
            this.DropIndex("dbo.Dialogs", new[] { "FirstUserId" });
            this.DropColumn("dbo.Dialogs", "SecondUserId");
            this.DropColumn("dbo.Dialogs", "FirstUserId");
            this.DropColumn("dbo.Messages", "ReadBy");
            this.RenameIndex(table: "dbo.Dialogs", name: "IX_User_Id", newName: "IX_StarterId");
            this.RenameIndex(table: "dbo.Messages", name: "IX_User_Id", newName: "IX_UserId");
            this.RenameColumn(table: "dbo.Dialogs", name: "User_Id", newName: "StarterId");
            this.RenameColumn(table: "dbo.Messages", name: "User_Id", newName: "UserId");
            this.CreateIndex("dbo.DialogParticipants", "UserId");
            this.CreateIndex("dbo.DialogParticipants", "DialogId");
            this.AddForeignKey("dbo.DialogParticipants", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            this.AddForeignKey("dbo.DialogParticipants", "DialogId", "dbo.Dialogs", "Id", cascadeDelete: true);
        }
    }
}
