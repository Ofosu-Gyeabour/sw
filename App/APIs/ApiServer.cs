using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.APIs.Response;
using App.POCOs;
using System.Diagnostics.Contracts;
using App.utils;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Net.Security;
using System.Net;

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
                HttpClient client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.MD5_AUTH), ConfigObject.CONTENT_TYPE);
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
        public HttpClient BuildHTTPClient(string URL, string _contentType)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, SslPolicyErrors) => { return true; };
            clientHandler.UseProxy = false;

            var client = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(URL)
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(_contentType));
            client.DefaultRequestHeaders.Add("accept", "*/*");
            

            return client;
        }
        public async Task<APIResponse> GetMD5EncryptionAsync(SingleValue payLoad)
        {
            APIResponse apiResponse = null;

            try
            {
                var client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.MD5_ENC), ConfigObject.CONTENT_TYPE);
                
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //ServicePointManager.Expect100Continue = false;

                var content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var response = await client.PostAsync(client.BaseAddress, content);
                    
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    apiResponse = JsonConvert.DeserializeObject<APIResponse>(body);
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
        public async Task<APIResponse> WriteLogAsync(Logger LoggerPayLoad)
        {
            APIResponse apiresponse = new APIResponse();

            try
            {
                var client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.WRITE_LOG), ConfigObject.CONTENT_TYPE);
                var content = new StringContent(JsonConvert.SerializeObject(LoggerPayLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var response = await client.PostAsync(client.BaseAddress, content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    apiresponse = JsonConvert.DeserializeObject<APIResponse>(responseBody);
                }

                return apiresponse;
            }
            catch(Exception x)
            {
                apiresponse = new APIResponse()
                {
                    status = false,
                    message = $"{x.Message}"
                };

                return apiresponse;
            }
        }
        public async Task<APIResponse> SetLoggedFlagAsync(App.POCOs.User _user, bool statusFlag)
        {
            APIResponse apiResponse = null;
            string url = string.Empty;

            if (statusFlag) { url = ConfigObject.LOGIN_FLAG; } else { url = ConfigObject.LOGOUT_FLAG; }

            try
            {
                HttpClient _client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, url), ConfigObject.CONTENT_TYPE);
                
                var content = new StringContent(JsonConvert.SerializeObject(_user), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var status = await _client.PostAsync(_client.BaseAddress, content);
                status.EnsureSuccessStatusCode();
                if (status.IsSuccessStatusCode)
                {
                    var body = await status.Content.ReadAsStringAsync();
                    apiResponse = JsonConvert.DeserializeObject<APIResponse>(body);
                }

                return apiResponse;
            }
            catch(Exception apiErr)
            {
                apiResponse = new APIResponse()
                {
                    status = false,
                    message = $"Error: {apiErr.Message} | inner exception: {apiErr.InnerException.Message}"
                };

                return apiResponse;
            }
            
        }
        public async Task<APIResponse> SaveProfileAsync(ProfileTemplate payLoad)
        {
            //method is used to post profile object to API
            APIResponse response = null;

            try
            {
                var client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.PROFILE_CREATE), ConfigObject.CONTENT_TYPE);
                var content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var resp = await client.PostAsync(client.BaseAddress, content);
                resp.EnsureSuccessStatusCode();
                if (resp.IsSuccessStatusCode)
                {
                    var responseBody = await resp.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<APIResponse>(responseBody);
                }

                return response;
            }
            catch(Exception apiErr)
            {
                return response = new APIResponse()
                {
                    status = false,
                    message = $"{apiErr.Message}"
                };
            }
        }
        public async Task<APIResponse> GetProfileListAsync(SingleValue _param)
        {
            //gets the list of profiles in a given company.
            APIResponse profileResponse = null;

            try
            {
                HttpClient client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.PROFILES_GET), ConfigObject.CONTENT_TYPE);
                var content = new StringContent(JsonConvert.SerializeObject(_param), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var response = await client.PostAsync(client.BaseAddress, content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    profileResponse = JsonConvert.DeserializeObject<APIResponse>(responseBody);
                }

                return profileResponse;
            }
            catch(Exception ex)
            {
                return profileResponse = new APIResponse() { status = false, message = $"{ex.Message}" };
            }
        }
        public async Task<APIResponse> GetUserProfileAsync(App.POCOs.User payLoad)
        {
            //gets the profile of a user
            var apiResponse = new APIResponse();
            var client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.USER_PROFILE_GET), ConfigObject.CONTENT_TYPE);
            var content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

            var response = await client.PostAsync(client.BaseAddress, content);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseBody);
            }

            return apiResponse;
        }
        public async Task<APIResponse> GetModulesOfProfileAsync(SingleValue payLoad)
        {
            APIResponse apiResponse = null;
            try
            {
                var client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.USER_PROFILE_MODULES), ConfigObject.CONTENT_TYPE);
                var content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var response = await client.PostAsync(client.BaseAddress, content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    apiResponse = JsonConvert.DeserializeObject<APIResponse>(responseBody);
                }

                return apiResponse;
            }
            catch(Exception err)
            {
                return apiResponse = new APIResponse() { status = false, message = $"{err.Message}" };
            }
        }
        public async Task<APIResponse> AmendUserProfileAsync(UserProfile payLoad)
        {
            APIResponse results = null;

            try
            {
                HttpClient _client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.USER_PROFILE_AMEND), ConfigObject.CONTENT_TYPE);
                var _content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var response = await _client.PostAsync(_client.BaseAddress, _content);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var returnedValue = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<APIResponse>(returnedValue);
                }

                return results;
            }
            catch(Exception x)
            {
                return results = new APIResponse() { status = false, data = $"{x.Message}"  };
            }
        }
        public async Task<APIResponse> GetDepartmentsAsync()
        {
            //method fetches departments from the web API
            var results = new APIResponse();

            try
            {
                var _client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.DEPARTMENT_API), ConfigObject.CONTENT_TYPE);

                var response = await _client.GetAsync(_client.BaseAddress);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<APIResponse>(body);
                }

                return results;
            }
            catch(Exception x)
            {
                return results = new APIResponse()
                {
                    status = false,
                    message = $"{x.Message}"
                };
            }
        }
        public async Task<APIResponse> CreateUserAccountAsync(userRecord payLoad)
        {
            //method creates user in the data store
            APIResponse response = null;
            try
            {
                HttpClient client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.USER_CREATE), ConfigObject.CONTENT_TYPE);
                var content = new StringContent(JsonConvert.SerializeObject(payLoad), Encoding.UTF8, ConfigObject.CONTENT_TYPE);

                var results = await client.PostAsync(client.BaseAddress, content);
                
                results.EnsureSuccessStatusCode();
                if (results.IsSuccessStatusCode)
                {
                    string body = await results.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<APIResponse>(body);
                }
                
                return response;
            }
            catch(Exception e)
            {
                return response = new APIResponse() { status = false, message = $"{e.Message}" };
            }
        }
    
        public async Task<APIResponse> GetUsersAsync()
        {
            var results = new APIResponse();

            try
            {
                HttpClient _client = BuildHTTPClient(string.Format("{0}{1}", ConfigObject.API_STUB, ConfigObject.USERS_API), ConfigObject.CONTENT_TYPE);
                _client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");

                var response = await _client.GetAsync(_client.BaseAddress);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string body = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<APIResponse>(body);
                }

                return results;
            }
            catch (Exception x)
            {
                return results = new APIResponse()
                {
                    status = false,
                    message = $"{x.Message}"
                };
            }
        }

    }
}
