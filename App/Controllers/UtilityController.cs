using App.APIs.Response;
using App.Session;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace App.Controllers
{
    public class UtilityController : Controller
    {
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
    }
}
