namespace Kztek.Data.Event.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblAlarm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        CardNumber = c.String(),
                        Plate = c.String(),
                        UserID = c.String(),
                        LaneID = c.String(),
                        PicDir = c.String(),
                        AlarmCode = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblCardEventHistory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventCode = c.String(),
                        CardNumber = c.String(),
                        DatetimeIn = c.DateTime(),
                        DateTimeOut = c.DateTime(),
                        PicDirIn = c.String(),
                        PicDirOut = c.String(),
                        LaneIDIn = c.String(),
                        LaneIDOut = c.String(),
                        UserIDIn = c.String(),
                        UserIDOut = c.String(),
                        PlateIn = c.String(),
                        PlateOut = c.String(),
                        RegistedPlate = c.String(),
                        Moneys = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CardGroupID = c.String(),
                        VehicleGroupID = c.String(),
                        CustomerGroupID = c.String(),
                        CustomerName = c.String(),
                        IsBlackList = c.Boolean(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        FreeType = c.String(),
                        CardNo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblCardEvent",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventCode = c.String(),
                        CardNumber = c.String(),
                        DatetimeIn = c.DateTime(),
                        DateTimeOut = c.DateTime(),
                        PicDirIn = c.String(),
                        PicDirOut = c.String(),
                        LaneIDIn = c.String(),
                        LaneIDOut = c.String(),
                        UserIDIn = c.String(),
                        UserIDOut = c.String(),
                        PlateIn = c.String(),
                        PlateOut = c.String(),
                        RegistedPlate = c.String(),
                        Moneys = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CardGroupID = c.String(),
                        VehicleGroupID = c.String(),
                        CustomerGroupID = c.String(),
                        CustomerName = c.String(),
                        IsBlackList = c.Boolean(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                        FreeType = c.String(),
                        CardNo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblChangeEvent",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        EventID = c.String(),
                        CardNumber = c.String(),
                        UserID = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tblDispenser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        ControllerID = c.String(),
                        ControllerName = c.String(),
                        State = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblLoopEvent",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        EventCode = c.String(),
                        Plate = c.String(),
                        DatetimeIn = c.DateTime(),
                        DatetimeOut = c.DateTime(),
                        PicDirIn = c.String(),
                        PicDirOut = c.String(),
                        UserIDIn = c.String(),
                        UserIDOut = c.String(),
                        LaneIDIn = c.String(),
                        LaneIDOut = c.String(),
                        CustomerName = c.String(),
                        IsEditPlateIn = c.Boolean(nullable: false),
                        IsEditPlateOut = c.Boolean(nullable: false),
                        Moneys = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsFree = c.Boolean(nullable: false),
                        FreeType = c.String(),
                        CarType = c.String(),
                        IsDelete = c.Boolean(nullable: false),
                        Voucher = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblPrintIndex",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        EventID = c.String(),
                        PrintIndex = c.Int(),
                        Para3 = c.String(),
                        Para1 = c.String(),
                        Para2 = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tblVoucher",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(),
                        EventID = c.String(),
                        Voucher = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblVoucher");
            DropTable("dbo.tblPrintIndex");
            DropTable("dbo.tblLoopEvent");
            DropTable("dbo.tblDispenser");
            DropTable("dbo.tblChangeEvent");
            DropTable("dbo.tblCardEvent");
            DropTable("dbo.tblCardEventHistory");
            DropTable("dbo.tblAlarm");
        }
    }
}
