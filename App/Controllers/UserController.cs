global using Microsoft.EntityFrameworkCore;
using App.POCOs;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class UserController : Controller
    {
        public async  Task<IActionResult> Login(string usrname, string pwd)
        {
            try
            {
                var objUser = new User() { username = usrname, password = pwd };

            }
            catch(Exception x)
            {
                return Json(new { status = false, message = $"{x.Message}" });
            }
        }
    }
}
