namespace FTPClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "SignUpDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Users", "LastPasswordChange", c => c.DateTime(precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "LastPasswordChange", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Users", "SignUpDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
