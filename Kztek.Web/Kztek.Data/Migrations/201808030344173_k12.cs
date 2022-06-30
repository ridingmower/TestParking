namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCustomer",
                c => new
                    {
                        CustomerID = c.Guid(nullable: false),
                        CustomerCode = c.String(),
                        CustomerName = c.String(),
                        Address = c.String(),
                        IDNumber = c.String(),
                        Mobile = c.String(),
                        CustomerGroupID = c.String(),
                        Description = c.String(),
                        EnableAccount = c.Boolean(nullable: false),
                        Account = c.String(),
                        Password = c.String(),
                        Avatar = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        CompartmentId = c.String(),
                        AccessLevelID = c.String(),
                        Finger1 = c.String(),
                        Finger2 = c.String(),
                        UserIDofFinger = c.Int(nullable: false),
                        AccessExpireDate = c.DateTime(nullable: false),
                        DevPass = c.String(),
                    })
                .PrimaryKey(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCustomer");
        }
    }
}
