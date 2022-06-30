namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCamera",
                c => new
                    {
                        CameraID = c.Guid(nullable: false),
                        CameraCode = c.String(),
                        CameraName = c.String(),
                        HttpURL = c.String(),
                        HttpPort = c.String(),
                        RtspPort = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        FrameRate = c.Int(),
                        Resolution = c.String(),
                        Channel = c.Int(),
                        CameraType = c.String(),
                        StreamType = c.String(),
                        SDK = c.String(),
                        Cgi = c.String(),
                        EnableRecording = c.Boolean(nullable: false),
                        PCID = c.String(),
                        PositionIndex = c.Int(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CameraID);
            
            CreateTable(
                "dbo.tblController",
                c => new
                    {
                        ControllerID = c.Guid(nullable: false),
                        ControllerCode = c.String(),
                        ControllerName = c.String(),
                        CommunicationType = c.Int(),
                        Comport = c.String(),
                        Baudrate = c.String(),
                        LineTypeID = c.Int(),
                        Reader1Type = c.Int(),
                        Reader2Type = c.Int(),
                        PCID = c.String(),
                        Address = c.Int(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ControllerID);
            
            CreateTable(
                "dbo.tblGate",
                c => new
                    {
                        GateID = c.Guid(nullable: false),
                        GateCode = c.String(),
                        GateName = c.String(),
                        Description = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GateID);
            
            CreateTable(
                "dbo.tblLane",
                c => new
                    {
                        LaneID = c.Guid(nullable: false),
                        LaneCode = c.String(),
                        LaneName = c.String(),
                        PCID = c.String(),
                        LaneType = c.Int(),
                        IsLoop = c.Boolean(nullable: false),
                        CheckPlateLevelIn = c.Int(nullable: false),
                        CheckPlateLevelOut = c.Int(nullable: false),
                        IsPrint = c.Boolean(nullable: false),
                        C1 = c.String(),
                        C2 = c.String(),
                        C3 = c.String(),
                        C4 = c.String(),
                        C5 = c.String(),
                        C6 = c.String(),
                        Controller = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsLED = c.Boolean(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        AccessForEachSide = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LaneID);
            
            CreateTable(
                "dbo.tblLED",
                c => new
                    {
                        LEDID = c.Int(nullable: false, identity: true),
                        LEDName = c.String(),
                        PCID = c.String(),
                        Comport = c.String(),
                        Baudrate = c.Int(),
                        SideIndex = c.Int(nullable: false),
                        Address = c.Int(nullable: false),
                        LedType = c.String(),
                        EnableLED = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LEDID);
            
            CreateTable(
                "dbo.tblPC",
                c => new
                    {
                        PCID = c.Guid(nullable: false),
                        ComputerCode = c.String(),
                        ComputerName = c.String(),
                        GateID = c.String(),
                        IPAddress = c.String(),
                        PicPathIn = c.String(),
                        PicPathOut = c.String(),
                        VideoPath = c.String(),
                        Description = c.String(),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PCID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblPC");
            DropTable("dbo.tblLED");
            DropTable("dbo.tblLane");
            DropTable("dbo.tblGate");
            DropTable("dbo.tblController");
            DropTable("dbo.tblCamera");
        }
    }
}
