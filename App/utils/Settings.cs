namespace App.utils
{
    public class Settings
    {
        public string apiStub { get; set; } = string.Empty;
        public string scaffoldDbContext { get; set; } = string.Empty;
        public string dbConnectionString { get; set; } = string.Empty;

        public string MD5AuthenticationEndPoint { get; set; } = string.Empty;
        public string getUsersEndPoint { get; set; } = string.Empty;
        public string getDepartmentsEndPoint { get; set; } = string.Empty;
        public string GetMD5Encryption { get; set; } = string.Empty;
        public string SetLoggedInFlag { get; set; } = string.Empty;
        public string SetLoggedOutFlag { get; set; } = string.Empty;
        public string WriteLog { get; set; } = string.Empty;

        public string contentType { get; set; } = string.Empty;
        public string CreateProfile { get; set; } = string.Empty;
        public string GetProfiles { get; set; } = string.Empty;

        public string GetUserProfile { get; set; } = string.Empty;
        public string GetProfileModules { get; set; } = string.Empty;
        public string AmendUserProfile { get; set; } = string.Empty;
        public string GetDepartments { get; set; } = string.Empty;
        public string CreateUser { get; set; } = string.Empty;


        //newly-added
        public string changePassword { get; set; } = string.Empty;
    }

    public class Events
    {
        public string auth { get; set; } = string.Empty;
        public string exit { get; set; } = string.Empty;
    }

}
