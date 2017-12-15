namespace FTPClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Directories", new[] { "ParentDirectoryId" });
            AlterColumn("dbo.Directories", "ParentDirectoryId", c => c.Int());
            CreateIndex("dbo.Directories", "ParentDirectoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Directories", new[] { "ParentDirectoryId" });
            AlterColumn("dbo.Directories", "ParentDirectoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Directories", "ParentDirectoryId");
        }
    }
}
