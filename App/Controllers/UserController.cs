global using Microsoft.EntityFrameworkCore;
using App.APIs;
using App.Helper;
using App.POCOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Session;
using App.APIs.Response;

namespace App.Controllers
{
    public class UserController : Controller
    {
        public async  Task<IActionResult> Login(string usrname, string pwd)
        {
            try
            {
                UserAPIResponse session = null;

                var objUser = new App.POCOs.User() { username = usrname, password = pwd };
                var param = new SingleValue() { stringValue = objUser.password };
                var api = new ApiServer();

                var encryptedObject = await api.GetMD5EncryptionAsync(param);
                var Cfg = new Utility();

                if (encryptedObject.status)
                {
                    var user_record = await Cfg.getUserRecordAsync(objUser.username);
                    var api_user_data = await api.AuthenticateUserAsync(objUser);

                    if (api_user_data.status)
                    {
                        if (encryptedObject.data.ToString() == user_record.password)
                        {
                            if (api_user_data.user.isActive == 1)
                            {
                                if (api_user_data.user.isLogged == 0)
                                {
                                    //get user privileges for session storage
                                    //session = api_user_data;
                                    HttpContext.Session.setObject("userSession", api_user_data);
                                    HttpContext.Session.SetInt32("userId", api_user_data.user.id);

                                    return Json(new { status = true, data = api_user_data });
                                }
                                else { return Json(new { status = false, reason = @"multiple_Logs" }); }
                            }
                            else { return Json(new { status = false, reason = @"inactive" }); }
                        }
                        else
                        {
                            return Json(new {status = false, reason = @"pwd"});
                        }   
                    }
                    else
                    {
                        return Json(new { status = false, reason = @"user" });
                    }
                }
                else { return Json(new { status = false, reason = @"something happened. Contact Administrator" }); }
            }
            catch(Exception x)
            {
                return Json(new { status = false, message = $"{x.Message}" });
            }
        }
    
        public async Task<JsonResult> Logout()
        {
            try
            {
                var session = HttpContext.Session.GetObject<UserAPIResponse>("userSession");
                int uId = int.Parse(HttpContext.Session.GetInt32("userId").ToString());

                HttpContext.Session.Clear();

                return Json(new { status = true, msg = string.Format("{0} logged out successfully",session.user.usrname) });
            }
            catch(Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return Json(new { status = true, msg = $"{x.Message}" });
            }
        }
    
        public IActionResult GetLoggedStatus()
        {
            try
            {
                var session = HttpContext.Session.GetObject<UserAPIResponse>("userSession");
                if (session.user.id == 0)
                {
                    //RedirectToAction("AltLogin", "Home");
                    HttpContext.Session.Clear();
                    return Json(new { status = false, data = $"logging out...redirecting" });
                }
                else { return Json(new { status = true }); }
            }
            catch(Exception x)
            {
                return Json(new {status = true, data = $"logged on..no redirection"});
            }
        }
    }
}
