namespace Kztek.Web.Core.Models
{
    public class SessionConfig
    {
        public const string MemberSession = "cp_memberSession";
        public const string MemberCookies = "cp_memberCookies";
        public const string MemberSystemSession = "cp_memberSystemSession";
        public const string MemberSystemCookies = "cp_memberSystemCookies";
        public const string WebInfoSession = "cp_webinfoSession";
        public const string SystemConfigSession = "cp_webinfoSession";
        public const string CustomerSession = "cp_customerSession";
        public const string CustomerCookies = "cp_customerCookies";
        public const string MemberManagerSession = "cp_memberManagerSession";
        public const string MemberManagerCookies = "cp_memberManagerCookies";

        //Super admin
        public const string SuperAdminSession = "cp_SuperAdminSession";

        //
        public const string CardActiveParkingSession = "CardActiveParkingSession";
        public const string CardDateActiveParkingSession = "CardDateActiveParkingSession";

        //
        public const string CardChoiceActionParkingSession = "CardChoiceActionParkingSession";
        public const string UserChoiceActionParkingSession = "UserChoiceActionParkingSession";
        public const string UserAdminChoiceActionParkingSession = "UserAdminChoiceActionParkingSession";

        //
        public const string CardChoiceActionAccessSession = "CardChoiceActionAccessSession";

        //
        public const string ControllerActiveAccessSession = "ControllerActiveAccessSession";
        public const string SelfHostActiveAccessSession = "SelfHostActiveAccessSession";
        public const string CardActiveAccessSession = "CardActiveAccessSession";
        public const string CustomerActiveAccessSession = "CustomerActiveAccessSession";

        //Chi tiết thẻ tháng trường chinh
        public const string EventIdActionParkingSession = "EventIdActionParkingSession";

        //Chi tiết thẻ lượt trường chinh
        public const string EventIdCardDayActionParkingSession = "EventIdCardDayActionParkingSession";

        //trả sau
        public const string EventIdPayLaterActionParkingSession = "EventIdPayLaterActionParkingSession";

        //Chi tiết thẻ lượt mien phi trường chinh
        public const string EventIdFreeAllActionParkingSession = "EventIdFreeAllActionParkingSession";

        //
        public const string ControllerLockerSession = "ControllerLockerSession";

        public const string SelfHostLockerSession = "SelfHostLockerSession";

        public const string CardLockerSession = "CardLockerSession";

        public const string LockerSession = "LockerSession";

        //danh sách event id delete 108
        public const string EventIdDelete108ParkingSession = "EventIdDelete108ParkingSession";

        public const string EventIdBachMaiParkingSession = "EventIdBachMaiParkingSession";
    }
}