namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCardGroup",
                c => new
                    {
                        CardGroupID = c.Guid(nullable: false),
                        CardGroupCode = c.String(),
                        CardGroupName = c.String(),
                        Description = c.String(),
                        CardType = c.Int(nullable: false),
                        VehicleGroupID = c.String(),
                        LaneIDs = c.String(),
                        DayTimeFrom = c.String(),
                        DayTimeTo = c.String(),
                        Formulation = c.Int(nullable: false),
                        EachFee = c.Int(nullable: false),
                        Block0 = c.Int(nullable: false),
                        Time0 = c.Int(nullable: false),
                        Block1 = c.Int(nullable: false),
                        Time1 = c.Int(nullable: false),
                        Block2 = c.Int(nullable: false),
                        Time2 = c.Int(nullable: false),
                        Block3 = c.Int(nullable: false),
                        Time3 = c.Int(nullable: false),
                        Block4 = c.Int(nullable: false),
                        Time4 = c.Int(nullable: false),
                        Block5 = c.Int(nullable: false),
                        Time5 = c.Int(nullable: false),
                        TimePeriods = c.String(),
                        Costs = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsHaveMoneyExcessTime = c.Boolean(nullable: false),
                        EnableFree = c.Boolean(nullable: false),
                        FreeTime = c.Int(nullable: false),
                        IsCheckPlate = c.Boolean(nullable: false),
                        IsHaveMoneyExpiredDate = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CardGroupID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCardGroup");
        }
    }
}
