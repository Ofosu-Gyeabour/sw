﻿namespace App.APIs.Response
{
    public class UserAPIResponse
    {
        public bool status { get; set; }
        public string message { get; set; }

        public User user { get; set; }
        public Company company { get; set; }
        public Profile profile { get; set; }
        public Department department { get; set; }
    }


    public class User
    {
        public int id { get; set; }
        public string surname { get; set; }
        public string firstname { get; set; }
        public string othernames { get; set; }
        public string usrname { get; set; }
        public string usrpassword { get; set; }
        public int? isAdmin { get; set; }
        public int? isLogged { get; set; }
        public int? isActive { get; set; }

    }

    public class Company
    {
        public int id { get; set; }
        public string company { get; set; }
        public string companyAddress { get; set; }
        public DateTime? incorporationDate { get; set; }

    }

    public class Profile
    {
        public int id { get; set; }
        public string? profileString { get; set; }
        public int? inUse { get; set; }
        public DateTime? dateAdded { get; set; }
    }

    public class Department
    {
        public int id { get; set; }
        public string departmentName { get; set; }
        public string departmentDescription { get; set; }
    }


}
