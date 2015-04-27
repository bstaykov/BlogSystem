namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostTitleIndex : DbMigration
    {
        public override void Up()
        {
            this.AlterColumn("dbo.Posts", "Title", title => title.String(maxLength: 50, nullable: false));
            this.AlterColumn("dbo.Posts", "Content", title => title.String(maxLength: 1000, nullable: false));
            this.CreateIndex("dbo.Posts", "Title", unique: true);
            this.CreateIndex("dbo.Posts", "Content", unique: true);
        }
        
        public override void Down()
        {
            this.DropIndex("dbo.Posts", new[] { "Title" });
            this.DropIndex("dbo.Posts", new[] { "Content" });
        }
    }
}
