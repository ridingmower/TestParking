using Kztek.Model.Models;
using Kztek.Model.Models.API;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Kztek.Data
{
    public class KztekEntities : DbContext
    {
        public KztekEntities()
            : base("KztekEntities")
        {
            Database.SetInitializer<KztekEntities>(null);
            this.Database.CommandTimeout = 180;
        }

        public KztekEntities(string conn) : base(conn)
        {
            Database.SetInitializer<KztekEntities>(null);
        }

        //Config
        public DbSet<MenuFunctionConfig> MenuFunctionConfigs { get; set; }
        public DbSet<SystemRecord> SystemRecords { get; set; }

        //Hệ thống
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<MenuFunction> MenuFunctions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<tblSystemConfig> tblSystemConfigs { get; set; }
        public DbSet<ExcelColumn> ExcelColumns { get; set; }

        //iParking
        public DbSet<tblGate> tblGates { get; set; }

          public DbSet<MultipleLanesMap> MultipleLanesMaps { get; set; }

         
      

        public DbSet<tblPC> tblPCs { get; set; }

        public DbSet<tblCamera> tblCameras { get; set; }

        public DbSet<tblController> tblControllers { get; set; }

        public DbSet<tblLane> tblLanes { get; set; }

        public DbSet<tblLED> tblLEDs { get; set; }

        public DbSet<tblBlackList> tblBlackLists { get; set; }

        public DbSet<tblCompartment> tblCompartments { get; set; }

        public DbSet<tblCardGroup> tblCardGroups { get; set; }

        public DbSet<tblFee> tblFees { get; set; }

        public DbSet<tblCard> tblCards { get; set; }

        public DbSet<tblCardProcess> tblCardProcesses { get; set; }

        public DbSet<tblActiveCard> tblActiveCards { get; set; }
        public DbSet<OrderActiveCard> OrderActiveCards { get; set; }
        //
        public DbSet<tblSubSystem> tblSubSystems { get; set; }
        public DbSet<tblSubCard> tblSubCards { get; set; }
        public DbSet<tblUser> tblUsers { get; set; }

        public DbSet<tblUserJoinRole> tblUserJoinRoles { get; set; }

        public DbSet<tblRole> tblRoles { get; set; }

        public DbSet<tblRolePermissionMaping> tblRolePermissionMapings { get; set; }

        //
        public DbSet<tblVehicleGroup> tblVehicleGroups { get; set; }

        public DbSet<tblDiscountParking> tblDiscountParkings { get; set; }
        //
        public DbSet<tblLog> tblLogs { get; set; }

        //
        public DbSet<tblCustomerGroup> tblCustomerGroups { get; set; }

        public DbSet<tblCustomer> tblCustomers { get; set; }
        public DbSet<DynamicCard> DynamicCards { get; set; }

        //iAccess
        public DbSet<tblAccessController> tblAccessControllers { get; set; }

        //public DbSet<tblAccessControllerMemory> tblAccessControllerMemories { get; set; }

        public DbSet<tblAccessDoor> tblAccessDoors { get; set; }

        public DbSet<tblAccessLevel> tblAccessLevels { get; set; }

        public DbSet<tblAccessLevelDetail> tblAccessLevelDetails { get; set; }

        public DbSet<tblAccessLine> tblAccessLines { get; set; }

        public DbSet<tblAccessPC> tblAccessPCs { get; set; }

        public DbSet<tblAccessTimezone> tblAccessTimezones { get; set; }

        public DbSet<tblAccessUploadDetail> tblAccessUploadDetails { get; set; }

        public DbSet<tblAccessUploadProcess> tblAccessUploadProcesses { get; set; }
        public DbSet<tblAccessControllerGroup> tblAccessControllerGroup { get; set; }
        public DbSet<tblAccessCamera> tblAccessCameras { get; set; }

        public DbSet<SelfHostConfig> SelfHostConfigs { get; set; }

        //
        public DbSet<tblLockerPC> tblLockerPCs { get; set; }

        public DbSet<tblLockerLine> tblLockerLines { get; set; }

        public DbSet<tblLockerController> tblLockerControllers { get; set; }

        public DbSet<tblLocker> tblLockers { get; set; }

        public DbSet<tblLockerSelfHost> tblLockerSelfHosts { get; set; }

        public DbSet<tblLockerProcess> tblLockerProcesses { get; set; }

        public DbSet<User_AuthGroup> User_AuthGroups { get; set; }

        //API
        public DbSet<API_Auth> API_Auths { get; set; }

        public DbSet<tblFtpAccount> tblFtpAccounts { get; set; }

        //Resident
        public DbSet<BM_ApartmentRole> BM_ApartmentRoles { get; set; }

        public DbSet<BM_Building_Service> BM_Building_Services { get; set; }

        public DbSet<BM_ApartmentEWPrice> BM_ApartmentEWPrices { get; set; }
        public DbSet<BM_ResidentGroup> BM_ResidentGroups { get; set; }

        public DbSet<BM_Resident> BM_Residents { get; set; }

        public DbSet<BM_Building> BM_Buildings { get; set; }
        public DbSet<BM_Floor> BM_Floors { get; set; }
        public DbSet<BM_Apartment_Receipt> BM_Apartment_Receipt { get; set; }
        public DbSet<BM_Apartment_Service> BM_Apartment_Service { get; set; }
        public DbSet<BM_Apartment_Member> BM_Apartment_Member { get; set; }
        public DbSet<BM_Apartment> BM_Apartment { get; set; }
        public DbSet<BM_ApartmentUse> BM_ApartmentUse { get; set; }
        public DbSet<tblCardSubmitEvent> tblCardSubmitEvent { get; set; }
        public DbSet<ExtendCard> ExtendCards { get; set; }
        public DbSet<OrderInfo> OrderInfos { get; set; }
        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
        }
    }
}
