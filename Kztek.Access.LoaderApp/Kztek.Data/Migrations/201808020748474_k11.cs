namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCustomerGroup",
                c => new
                    {
                        CustomerGroupID = c.Guid(nullable: false),
                        ParentID = c.String(),
                        CustomerGroupCode = c.String(),
                        CustomerGroupName = c.String(),
                        Description = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerGroupID);
            
            CreateTable(
                "dbo.tblLog",
                c => new
                    {
                        LogID = c.Guid(nullable: false),
                        Date = c.DateTime(),
                        UserName = c.String(),
                        AppCode = c.String(),
                        SubSystemCode = c.String(),
                        ObjectName = c.String(),
                        Actions = c.String(),
                        Description = c.String(),
                        IPAddress = c.String(),
                        ComputerName = c.String(),
                    })
                .PrimaryKey(t => t.LogID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblLog");
            DropTable("dbo.tblCustomerGroup");
        }
    }
}
