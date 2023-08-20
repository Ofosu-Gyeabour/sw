using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.APIs.Response;
using App.POCOs;
using System.Diagnostics.Contracts;
using App.utils;

namespace App.APIs
{
    public class ApiServer
    {
        public ApiServer() { }

        public async Task<UserAPIResponse> AuthenticateUserAsync(App.POCOs.User payLoad)
        {
            //method consumes an endpoint that authenticates user using the MD5 encryption algorithm
            UserAPIResponse apiResponse = null;

            try
            {
                var client = new HttpClient() { 
                    BaseAddress = new Uri(ConfigObject.MD5_AUTH)
                };

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(ConfigObject.CONTENT_TYPE));
                var content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var response = await client.PostAsync(client.BaseAddress, content);

                using (response)
                {
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        apiResponse = JsonConvert.DeserializeObject<UserAPIResponse>(responseBody);
                    }
                    else { 
                        apiResponse = new UserAPIResponse() { status = false }; 
                    }
                }

                return apiResponse;
            }
            catch(Exception x)
            {
                apiResponse = new UserAPIResponse() { 
                    status = false,
                    message = $"{x.Message}"
                };

                return apiResponse;
            }
        }

        public async Task<APIResponse> GetMD5EncryptionAsync(SingleValue payLoad)
        {
            APIResponse apiResponse = null;

            var client = new HttpClient()
            {
                BaseAddress = new Uri(ConfigObject.MD5_ENC)
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(ConfigObject.CONTENT_TYPE));
            var content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

            try
            {
                var response = await client.PostAsync(client.BaseAddress, content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    apiResponse =  JsonConvert.DeserializeObject<APIResponse>(responseBody);
                }

                return apiResponse;
            }
            catch(Exception x)
            {
                apiResponse = new APIResponse() { 
                    status = false,
                    message = $"{x.Message}"
                };

                return apiResponse;
            }

        }

    }
}
