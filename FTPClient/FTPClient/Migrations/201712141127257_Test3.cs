namespace FTPClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "UploadTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "UploadTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
