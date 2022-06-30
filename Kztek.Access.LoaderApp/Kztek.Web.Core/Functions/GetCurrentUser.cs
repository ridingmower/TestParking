using Kztek.Model.Models;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using System.Linq;
using System.Web;

namespace Kztek.Web.Core.Functions
{
    public class GetCurrentUser
    {
        public static bool CheckCurrentLogin()
        {
            var _user = GetUser();
            return _user == null ? false : true;
        }

        public static User GetUser()
        {
            var host = HttpContext.Current.Request.Url.Host;
            var _user = (User)HttpContext.Current.Session[string.Format("{0}_{1}", SessionConfig.MemberSession, host)];
            //lay user theo session
            //neu cos thi tra ve user
            //Neu khong co thi tim xem cookie co luu khong
            //Neu luu duoi cookie thi lay thong tin len gan vao session
            //Neu khong luu o cookie tra ve null
            if (_user == null)
            {
                //get by cookie
                var memberCookies = HttpContext.Current.Request.Cookies[string.Format("{0}_{1}", SessionConfig.MemberCookies, host)];
                if (memberCookies != null)
                {
                    using (var db = new Kztek.Data.KztekEntities())
                    {
                        var userId = memberCookies["cp_UserId"];

                        var formatUserId = string.Format("{0}_{1}", ConstField.MemCacheMember, userId);
                        if (CacheLayer.Exists(formatUserId))
                        {
                            var objUser = CacheLayer.Get<User>(formatUserId);

                            _user = objUser;
                        }
                        else
                        {
                            var memberInfo = db.Users.FirstOrDefault(c => c.Id.Equals(userId));
                            if (memberInfo != null)
                            {
                                _user = new User
                                {
                                    Id = memberInfo.Id,
                                    Name = memberInfo.Name,
                                    Email = memberInfo.Email,
                                    Admin = memberInfo.Admin,
                                    UserAvatar = memberInfo.UserAvatar,
                                    Username = memberInfo.Username
                                };
                            }
                            CacheLayer.Add(formatUserId, _user, ConstField.TimeCache);
                        }
                    }

                }
            }
            return _user;
        }
    }
}
