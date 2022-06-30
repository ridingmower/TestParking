namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k15 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tblCustomer", "AccessExpireDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tblCustomer", "AccessExpireDate", c => c.DateTime(nullable: false));
        }
    }
}
