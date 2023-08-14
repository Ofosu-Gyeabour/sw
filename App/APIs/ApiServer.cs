using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.APIs.Response;
using App.POCOs;

namespace App.APIs
{
    public class ApiServer
    {
        public ApiServer() { };

        public async Task<APIResponse> AuthenticateUserAsync(User payLoad)
        {
            //method consumes an endpoint that authenticates user using the MD5 encryption algorithm

        }

    }
}
