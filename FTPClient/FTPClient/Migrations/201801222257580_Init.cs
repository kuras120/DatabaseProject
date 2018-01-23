namespace FTPClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Directories", new[] { "ParentDirectoryId" });
            AlterColumn("dbo.Directories", "ParentDirectoryId", c => c.Int());
            AlterColumn("dbo.Users", "SignUpDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Users", "LastPasswordChange", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Files", "UploadTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            CreateIndex("dbo.Directories", "ParentDirectoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Directories", new[] { "ParentDirectoryId" });
            AlterColumn("dbo.Files", "UploadTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Users", "LastPasswordChange", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Users", "SignUpDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Directories", "ParentDirectoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Directories", "ParentDirectoryId");
        }
    }
}
