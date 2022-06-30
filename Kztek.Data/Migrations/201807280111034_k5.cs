namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblBlackList",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Plate = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblCompartment",
                c => new
                    {
                        CompartmentID = c.Guid(nullable: false),
                        CompartmentName = c.String(),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CompartmentID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblCompartment");
            DropTable("dbo.tblBlackList");
        }
    }
}
