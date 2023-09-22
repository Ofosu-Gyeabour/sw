using App.APIs;
using App.APIs.Response;
using App.Helper;
using App.POCOs;
using App.Session;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;


namespace App.Controllers
{
    public class UtilityController : Controller
    {
        ApiServer api;
        Utility utils;

        public UtilityController()
        {
            api = new ApiServer();
            utils = new Utility();
        }
        public async Task<IActionResult> GetAssignedModules()
        {
            try
            {
                var session = HttpContext.Session.GetObject<UserAPIResponse>("userSession");
                var usModules = session.profile.profileString.Split('|');

                return Json(usModules);
            }
            catch(Exception x)
            {
                Debug.Print(x.Message);
                return Json(new { status = false, error = $"error: {x.Message}" });
            }
        }

        public JsonResult GetCurrentUser()
        {
            //method fetches the current user from the session object
            try
            {
                var session = HttpContext.Session.GetObject<UserAPIResponse>("userSession");
               
                return Json(new { status = true, dta = session.user });
            }
            catch(Exception x)
            {
                return Json(new { status = false, error = $"error: {x.Message}" });
            }
        }

        public async Task<JsonResult> getActiveModules()
        {
            //method gets all active modules
            try
            {
                var utils = new Utility();
                var modules = await utils.getActiveModulesAsync();

                return Json(new { status = true, data =modules });
            }
            catch(Exception x)
            {
                return Json(new { status = false, error = $"error: {x.Message}" });
            }
        }

        public async Task<IActionResult> getCurrentProfileOfUser(string u)
        {
            //gets the current profile of the user name supplied
            var _user = new App.POCOs.User() { username = u };
            try
            {
                var results = await api.GetUserProfileAsync(_user);
                var d = JsonConvert.DeserializeObject<ProfileTemplate>(results.data.ToString());

                return Json(new { status = results.status, msg = d.nameOfProfile });
            }
            catch(Exception x)
            {
                return Json(new { status = false, msg = $"{x.Message}" });
            }
        }

        public async Task<JsonResult> GetProfileModules(string _uProfile)
        {
            //gets the modules for a given profile
            var results = new List<moduleLookup>();
            try
            {
                var obj = new SingleValue() { stringValue = _uProfile };
                var module_response = await api.GetModulesOfProfileAsync(obj);

                var dbmodules = JsonConvert.DeserializeObject<IEnumerable<TModule>>(module_response.data.ToString());

                if ((dbmodules != null) & (dbmodules.Count() > 0))
                {
                    foreach(var item in dbmodules)
                    {
                        var mdl = new moduleLookup()
                        {
                            id = item.ModuleId,
                            module = item.SysName,
                            describ = item.PublicName
                        };

                        results.Add(mdl);
                    }
                }

                return Json(new { status = true, msg = results });
            }
            catch(Exception x)
            {
                return Json(new { status = false, msg = $"{x.Message}" });
            }
        }
    
        public async Task<JsonResult> GetDepartments()
        {
            //method gets departments from apiServer
            IEnumerable<departmentLookup> depts = null;

            try
            {
                var dta = await api.GetDepartmentsAsync();
                depts = JsonConvert.DeserializeObject<IEnumerable<departmentLookup>>(dta.data.ToString());

                return Json(new { status = dta.status, data = depts });
            }
            catch(Exception x)
            {
                return Json(new { status = false, msg = $"{x.Message}" });
            }
        }

    }
}
