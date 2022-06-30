namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SystemRecord",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Filename = c.String(),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SystemRecord");
        }
    }
}
