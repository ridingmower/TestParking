namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCard",
                c => new
                    {
                        CardID = c.Guid(nullable: false),
                        CardNo = c.String(),
                        CardNumber = c.String(),
                        CustomerID = c.String(),
                        CardGroupID = c.String(),
                        ImportDate = c.DateTime(),
                        ExpireDate = c.DateTime(),
                        Plate1 = c.String(),
                        VehicleName1 = c.String(),
                        Plate2 = c.String(),
                        VehicleName2 = c.String(),
                        Plate3 = c.String(),
                        VehicleName3 = c.String(),
                        IsLock = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        Description = c.String(),
                        DateRegister = c.DateTime(),
                        DateRelease = c.DateTime(),
                        DateCancel = c.DateTime(),
                        AccessLevelID = c.String(),
                        ChkRelease = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CardID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCard");
        }
    }
}
