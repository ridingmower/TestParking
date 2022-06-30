namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblVehicleGroup",
                c => new
                    {
                        VehicleGroupID = c.Guid(nullable: false),
                        VehicleGroupCode = c.String(),
                        VehicleGroupName = c.String(),
                        VehicleType = c.Int(),
                        LimitNumber = c.Int(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VehicleGroupID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblVehicleGroup");
        }
    }
}
