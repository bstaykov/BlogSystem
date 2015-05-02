namespace BlogSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleLength50 : DbMigration
    {
        public override void Up()
        {
            this.DropIndex("dbo.Posts", "UniqueTitle");
            this.AlterColumn("dbo.Posts", "Title", c => c.String(maxLength: 50));
            this.CreateIndex("dbo.Posts", "Title", unique: true, name: "UniqueTitle");
        }
        
        public override void Down()
        {
            this.DropIndex("dbo.Posts", "UniqueTitle");
            this.AlterColumn("dbo.Posts", "Title", c => c.String(maxLength: 20));
            this.CreateIndex("dbo.Posts", "Title", unique: true, name: "UniqueTitle");
        }
    }
}
