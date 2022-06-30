namespace Kztek.Web.Core.Models
{
    public class ConstField
    {
        #region Memory cache name

        public const string MemCacheMember = "memCacheMember";
        public const string MemCachePermissions = "memCachePermissions";

        public const string AllListMenuFunctionCache = "allListMenuFunctionCache";

        public const string AllListStationCache = "allListStationCache";

        public const string ListUserRole = "listUserRole";

        public const string ListUserStation = "listUserStation";

        public const string ListRoleMenu = "listRoleMenu";

        public const string ListLane = "listLane";

        public const string ListGate = "listGate";

        public const string ListPC = "listPC";

        public const string ListUser = "listUser";

        public const string ListCompany = "listCompany";

        public const string ListRoute = "listRoute";

        public const string ListServicePriceType = "listServicePriceType";

        public const string WebConfigObj = "WebConfigObj";
        #endregion Memory cache name

        #region TimeCache

        public const int TimeCache = 86400;

        #endregion TimeCache

        #region PageSize

        public const int PageSizeDefault = 25;

        #endregion PageSize

        #region Sessions

        public const string Popup = "Popup";

        #endregion Sessions

        #region BMS Group

        //Tòa nhà
        public const string BuildingID = "67810176";

        //Vào ra
        public const string AccessControlID = "98818976";

        //Bãi xe
        public const string ParkingID = "12878956";

        //Tủ đồ
        public const string LockerID = "61119719";

        //cư dân
        public const string ResidentID = "75675733";

        public const string ResidentCode = "Resident";

        public const string ParkingCode = "Parking";
        public const string ApiParking = "APIParking";


        public const string AccessControlCode = "AccessControl";


        public const string LockerCode = "Locker";

        #endregion BMS Group
    }
}