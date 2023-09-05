namespace App.POCOs
{
    public class User
    {
        public string username { get; set; } = string.Empty;
        public string? password { get; set; } = string.Empty;
    }

    public class SingleValue
    {
        public string stringValue { get; set; } = string.Empty;
    }

    public class ProfileTemplate
    {
        public int id { get; set; } = 0;
        public string profileModules { get; set; } = string.Empty;
        public string nameOfProfile { get; set; } = string.Empty;
        public int companyId { get; set; } = 1;
        public int inUse { get; set; } = 1;
        public DateTime dateAdded { get; set; } = DateTime.Now;
    }

    public class UserProfile
    {
        public string username { get; set; } = string.Empty;
        public ProfileTemplate profile { get; set; }
    }

    public class departmentLookup
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string describ { get; set; } = string.Empty;
        public string companyName { get; set; } = string.Empty;
    }

    public class userRecord
    {
        public int id { get; set; } = 0;
        public string sname { get; set; }
        public string fname { get; set; }
        public string othernames { get; set; }
        public User userCredentials { get; set; }
        public int companyId { get; set; } = 1;
        public int departmentid { get; set; } = 1;
        public int isAdministrator { get; set; } = 1;
        public int isLogged { get; set; } = 0;
        public int isActive { get; set; } = 1;
        public int profileid { get; set; } = 2;
    }

}
