namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostReaders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostReaders",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PostId, t.UserId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            this.AddColumn("dbo.Posts", "TimesRead", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            this.DropForeignKey("dbo.PostReaders", "UserId", "dbo.AspNetUsers");
            this.DropForeignKey("dbo.PostReaders", "PostId", "dbo.Posts");
            this.DropIndex("dbo.PostReaders", new[] { "UserId" });
            this.DropIndex("dbo.PostReaders", new[] { "PostId" });
            this.DropColumn("dbo.Posts", "TimesRead");
            this.DropTable("dbo.PostReaders");
        }
    }
}
