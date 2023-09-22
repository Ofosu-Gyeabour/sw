global using Microsoft.EntityFrameworkCore;
using App.APIs;
using App.Helper;
using App.POCOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Session;
using App.APIs.Response;
using Newtonsoft.Json;
using App.utils;
using App.Models;

namespace App.Controllers
{
    public class UserController : Controller
    {
        ApiServer api;
        Utility utils;

        public UserController()
        {
            api = new ApiServer();
            utils = new Utility();
        }
        public async  Task<IActionResult> Login(string usrname, string pwd)
        {
            try
            {
               // return Json(new { status = true });

                UserAPIResponse session = null;

                var objUser = new App.POCOs.User() { username = usrname, password = pwd };
                var param = new SingleValue() { stringValue = objUser.password };

                var encryptedObject = await api.GetMD5EncryptionAsync(param);
                //var encryptedObject = utils.HashPassword(param);

                if (encryptedObject.status)
                {
                    objUser.password = encryptedObject.data.ToString();

                    //var user_record = await Cfg.getUserRecordAsync(objUser.username);

                    /*use the following until responseCache is sorted*/
                    //var api_user_data = await utils.GetUserAsync(objUser);

                    //api call..check responseCache header
                    var api_user_data = await api.AuthenticateUserAsync(objUser);

                    if (api_user_data.status)
                    {
                        if (api_user_data.user.isActive == 1)
                        {
                            //if (api_user_data.user.isLogged == 0)
                            //{
                                var profile = await utils.getProfileLookupAsync();

                                HttpContext.Session.setObject("userSession", api_user_data);
                                HttpContext.Session.SetInt32("userId", api_user_data.user.id);
                                HttpContext.Session.setObject("systemProfile", profile);

                                //log user activity
                                var evObj = await utils.getEventLookupAsync(ConfigObject.AUTH_OPERATION);

                                var objLog = new Logger()
                                {
                                    eventId = evObj.Id,
                                    actor = api_user_data.user.usrname,
                                    entity = @"User",
                                    entityValue = JsonConvert.SerializeObject(api_user_data.user),
                                    companyId = api_user_data.company.id,
                                    logDate = DateTime.Now
                                };

                                await api.WriteLogAsync(objLog);
                                await api.SetLoggedFlagAsync(objUser, true);
                                return Json(new { status = true, data = api_user_data });
                            //}
                            //else { return Json(new { status = false, reason = @"multiple_Logs" }); }
                        }
                        else { return Json(new { status = false, reason = @"inactive" }); }                         
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

                var objUser = new App.POCOs.User() {
                    username = session.user.usrname,
                    password = session.user.usrpassword
                };

                //log user activity
                var evntObj = await utils.getEventLookupAsync(ConfigObject.EXIT_OPERATION);
                var objLog = new Logger()
                {
                    eventId = evntObj.Id,
                    actor = session.user.usrname,
                    entity = @"User",
                    entityValue = JsonConvert.SerializeObject(session.user),
                    companyId = session.company.id,
                    logDate = DateTime.Now
                };

                await api.WriteLogAsync(objLog);
                await api.SetLoggedFlagAsync(objUser, false);
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
                if (session == null)
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
        public async Task<IActionResult> saveUserProfile(string _profile, string _profContent)
        {
            try
            {
                var session = HttpContext.Session.GetObject<UserAPIResponse>("userSession");
                var api = new ApiServer();

                var objProfile = new ProfileTemplate() { 
                    nameOfProfile = _profile,
                    profileModules = _profContent,
                    companyId = session.company.id,
                    inUse = 1,
                    dateAdded = DateTime.Now
                };

                var apiStatus = await api.SaveProfileAsync(objProfile);

                return Json(new { status = apiStatus.status, data = apiStatus });
            }
            catch (Exception exc)
            {
                return Json(new { status = false, message = $"{exc.Message}" });
            }
        }
        public async Task<JsonResult> getProfiles()
        {
            //if user is super admin, get profiles for ALL companies
            IEnumerable<profileLookup> profileList = new List<profileLookup>();

            try
            {
                var session = HttpContext.Session.GetObject<UserAPIResponse>("userSession");

                profileList = await utils.getProfileLookupAsync();
                if (profileList != null)
                {
                    if (session.user.isAdmin == 1)
                    {
                        HttpContext.Session.setObject("systemProfile", profileList);
                    }
                    else
                    {
                        //get profiles created in the company
                        List<profileLookup> result = new List<profileLookup>();
                        foreach(var p in profileList)
                        {
                            if (p.companyId == session.company.id)
                            {
                                result.Add(p);
                            }
                        }

                        //set result to profileList
                        profileList = result;
                    }
                }
                return Json(new { status = true, msg = profileList } );
            }
            catch(Exception x)
            {
                return Json(new { status = false, msg = $"{x.Message}" });
            }
        }
        public async Task<JsonResult> AmendUserProfile(string u, string pro)
        {
            //method is responsible for amending a user's profile
            UserProfile obj = new UserProfile()
            {
                username = u.Trim(),
                profile = new ProfileTemplate()
                {
                    nameOfProfile = pro
                }
            };

            try
            {
                //var apiResponse = await api.AmendUserProfileAsync(obj);
                var resp = await utils.AmendUserModules(obj.username, obj.profile.nameOfProfile);
                return Json(new { status = resp, message = $"{obj.username}'s profile amended successfully" });
            }
            catch(Exception x)
            {
                return Json(new { status = false, msg = $"{x.Message}" });
            }
        }
        public async Task<JsonResult> saveUserCredentials(string sname, string fname, string onames, string usr, string pwd,
                                                            string stat, string isAdm,string prof, int dept)
        {
            //method saves the user accounts in the data store
            try
            {
                var encrypted = await api.GetMD5EncryptionAsync(new SingleValue() { stringValue = pwd });
                var profileObj = await utils.getProfileRecordAsync(prof);

                var obj = new userRecord() { 
                    sname = sname.Trim(),
                    fname = fname.Trim(),
                    othernames = onames.Trim(),
                    userCredentials = new POCOs.User()
                    {
                        username = usr,
                        password = encrypted.data.ToString(),
                    },
                    companyId = 1,
                    departmentid = dept,
                    isAdministrator = isAdm == @"YES"? 1:0,
                    isLogged = 0,
                    isActive = stat == @"ACTIVE"? 1:0,
                    profileid = profileObj.ProfileId
                };

                var op_status = await api.CreateUserAccountAsync(obj);

                return Json(op_status);
            }
            catch(Exception x)
            {
                return Json(new { status = false, msg = $"{x.Message}" });
            }
        }

    
        public async Task<JsonResult> GetUserList()
        {
            //get users
            APIResponse usr_response = null;
            TProfile pObj = new TProfile();
            List<userLookup> users = null;

            try
            {
                usr_response = await api.GetUsersAsync();
                var user_list = JsonConvert.DeserializeObject<IEnumerable<Tusr>>(usr_response.data.ToString());               

                if (user_list.Count() > 0)
                {
                    users = new List<userLookup>();
                    foreach(var u in user_list)
                    {
                        if (u.ProfileId != null)
                        {
                            pObj = await utils.getProfileRecordAsync((int)u.ProfileId);

                            var obj = new userLookup()
                            {
                                id = u.UsrId,
                                usrname = u.Usrname,
                                active = u.IsActive == 1 ? "Yes" : "No",
                                logged = u.IsLogged == 1 ? "Yes" : "No",
                                isAdmin = u.IsAdmin == 1 ? "Yes" : "No",
                                profile = pObj.ProfileName
                            };

                            users.Add(obj);
                        }
                        
                    }
                }

                return Json(new { status = usr_response.status, data = users });
            }
            catch(Exception e)
            {
                return Json(new { status = false, msg = $"{e.Message}" });
            }
        }
    
        public async Task<JsonResult> changePassword(string usrname, string pwd,string flag)
        {
            //updates the password of the current user
            try
            {
                SingleValue param = new SingleValue() { stringValue = pwd };

                App.POCOs.User user = new POCOs.User()
                {
                    username = usrname,
                    password = utils.HashPassword(param)
                };

                var operationStatus = await api.GenericAPICallerAsync($"{ConfigObject.API_STUB}{ConfigObject.PWD_CHANGE}", user,ConfigObject.CONTENT_TYPE,@"put");
                if ((operationStatus.status) && (flag == @"self"))
                {
                    //update session object
                    var session = HttpContext.Session.GetObject<UserAPIResponse>("userSession");
                    session.user.usrpassword = user.password;

                    HttpContext.Session.Clear();
                    HttpContext.Session.setObject("userSession", session);
                }
                return Json(new { status = operationStatus.status, msg = operationStatus.message});
            }
            catch(Exception x)
            {
                return Json(new { status = false,error = $"error: {x.Message}" });
            }
        }
    }
}
