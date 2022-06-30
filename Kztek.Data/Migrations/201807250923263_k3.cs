namespace Kztek.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblRolePermissionMaping",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        RoleID = c.String(),
                        SubSystemID = c.String(),
                        Selects = c.Boolean(nullable: false),
                        Inserts = c.Boolean(nullable: false),
                        Updates = c.Boolean(nullable: false),
                        Deletes = c.Boolean(nullable: false),
                        Exports = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tblRole",
                c => new
                    {
                        RoleID = c.Guid(nullable: false),
                        RoleCode = c.String(),
                        RoleName = c.String(),
                        Description = c.String(),
                        IsSystem = c.Boolean(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        AppCode = c.String(),
                    })
                .PrimaryKey(t => t.RoleID);
            
            CreateTable(
                "dbo.tblSubSystem",
                c => new
                    {
                        SubSystemID = c.Guid(nullable: false),
                        ParentID = c.String(),
                        SubSystemCode = c.String(),
                        SubSystemName = c.String(),
                        SortOrder = c.Int(nullable: false),
                        Selects = c.Boolean(nullable: false),
                        Inserts = c.Boolean(nullable: false),
                        Updates = c.Boolean(nullable: false),
                        Deletes = c.Boolean(nullable: false),
                        Exports = c.Boolean(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        AppCode = c.String(),
                    })
                .PrimaryKey(t => t.SubSystemID);
            
            CreateTable(
                "dbo.tblUserJoinRole",
                c => new
                    {
                        UserJoinRoleID = c.Guid(nullable: false),
                        UserID = c.String(),
                        RoleID = c.String(),
                    })
                .PrimaryKey(t => t.UserJoinRoleID);
            
            CreateTable(
                "dbo.tblUser",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        UserCode = c.String(),
                        FullName = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        IsLock = c.Boolean(),
                        Avatar = c.String(),
                        SortOrder = c.Int(nullable: false),
                        IsSystem = c.Boolean(nullable: false),
                        CardGroupIds = c.String(),
                        CustomerGroupIds = c.String(),
                        AccessControllerSelected = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblUser");
            DropTable("dbo.tblUserJoinRole");
            DropTable("dbo.tblSubSystem");
            DropTable("dbo.tblRole");
            DropTable("dbo.tblRolePermissionMaping");
        }
    }
}
