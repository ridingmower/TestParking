namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuFunction",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MenuName = c.String(nullable: false),
                        ControllerName = c.String(maxLength: 150),
                        MenuType = c.String(maxLength: 10),
                        ActionName = c.String(maxLength: 150),
                        Url = c.String(maxLength: 1000),
                        Icon = c.String(maxLength: 100),
                        ParentId = c.String(maxLength: 100),
                        Active = c.Boolean(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        OrderNumber = c.Int(),
                        Breadcrumb = c.String(),
                        Dept = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleMenu",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MenuId = c.String(maxLength: 150, unicode: false),
                        RoleId = c.String(maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RoleName = c.String(maxLength: 150),
                        Description = c.String(maxLength: 250),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(maxLength: 150, unicode: false),
                        RoleId = c.String(maxLength: 150, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Email = c.String(maxLength: 250),
                        ImagePath = c.String(maxLength: 500, unicode: false),
                        Username = c.String(nullable: false, maxLength: 100),
                        Password = c.String(maxLength: 500, unicode: false),
                        PasswordSalat = c.String(maxLength: 500, unicode: false),
                        Address = c.String(maxLength: 500),
                        Phone = c.String(maxLength: 150, unicode: false),
                        Admin = c.Boolean(nullable: false),
                        Active = c.Boolean(nullable: false),
                        UserAvatar = c.String(),
                        DateCreated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
            DropTable("dbo.RoleMenu");
            DropTable("dbo.MenuFunction");
        }
    }
}
