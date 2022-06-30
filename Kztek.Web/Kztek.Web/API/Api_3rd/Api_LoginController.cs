using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kztek.Web.API
{
    //[Authorize]
    [RoutePrefix("api/Api_3rdLogin")]
    public class Api_LoginController : ApiController
    {
        IUserService _UserService;

        public Api_LoginController(IUserService _UserService)
        {
            this._UserService = _UserService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<MessageReport> User_Login(string Username, string Password)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                //Kiểm tra đúng hệ thống
                //if (model.KeyPass != ApiConfig.Key_System)
                //{
                //    result = new MessageReport(false, "Kết nối không hợp lệ");
                //    return await Task.FromResult(result);
                //}

                //Kiểm tra user tồn tại
                var user = await Task.FromResult(_UserService.GetByUserName(Username));
                if (user == null)
                {
                    result = new MessageReport(false, "Tài khoản không tồn tại");
                    return await Task.FromResult(result);
                }

                //Kiểm tra khóa
                if (user.Active == false)
                {
                    result = new MessageReport(false, "Tài khoản bị khóa");
                    return await Task.FromResult(result);
                }

                //Kiểm tra mk
                var pass = Password.PasswordHashed(user.PasswordSalat);
                if (user.Password != pass)
                {
                    result = new MessageReport(false, "Mật khẩu không khớp");
                    return await Task.FromResult(result);
                }

                var token = ApiHelper.GenerateJSON_MobileToken(user.Id);

                result = new MessageReport(true, token);
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }


            return await Task.FromResult(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("servertime")]
        public async Task<String> ServerTime()
        {
            var st = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return await Task.FromResult(st);
        }

        


    }
}