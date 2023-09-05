#nullable disable

using App.APIs.Response;
using App.POCOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Cryptography;
using System.Text;

namespace App.Helper
{
    public class Utility
    {
        swContext _context;
        public Utility() {
            _context = new swContext();
        }

        public async Task<App.POCOs.User> getUserRecordAsync(string _usrname)
        {
            //method gets a record of the user and the associated password
            App.POCOs.User result = null;

            try
            {
                var dta = await _context.Tusrs.Where(x => x.Usrname == _usrname).FirstOrDefaultAsync();
                if (dta != null)
                {
                    result = new App.POCOs.User()
                    {
                        username = _usrname,
                        password = dta.Usrpassword
                    };
                }

                return result;
            }
            catch(Exception x)
            {
                return result;
            }
        }

        public async Task<TEvent> getEventLookupAsync(string _evtDescription)
        {
            //gets the event object using the id
            TEvent objEvent = null;
            try
            {
                var obj = await _context.TEvents.Where(x => x.EventDescription == _evtDescription).FirstOrDefaultAsync();
                if (obj.Id > 0)
                {
                    objEvent = obj;
                }

                return objEvent;
            }
            catch(Exception x)
            {
                Debug.Print(x.Message);
                return objEvent;
            }
        }

        public async Task<IEnumerable<moduleLookup>> getActiveModulesAsync()
        {
            List<moduleLookup> modules = null;

            try
            {
                var module_list = await _context.TModules.Where(x => x.InUse == 1).ToListAsync();

                if (module_list.Count()> 0)
                {
                    modules = new List<moduleLookup>();

                    foreach(var m in module_list)
                    {
                        var obj = new moduleLookup() { 
                            id = m.ModuleId,
                            module = m.PublicName,
                            describ = m.SysName
                        };

                        modules.Add(obj);
                    }
                }

                return modules.ToList();
            }
            catch(Exception x)
            {
                Debug.Print(x.Message);
                return modules ;
            }
        }

        #region Profile

        public async Task<TProfile> getProfileRecordAsync(string profile)
        {
            //gets profile record
            TProfile obj = null;

            try
            {
                return await _context.TProfiles.Where(p => p.ProfileName == profile).FirstOrDefaultAsync();
            }
            catch(Exception x)
            {
                throw;
            }
        }

        public async Task<TProfile> getProfileRecordAsync(int profileId)
        {
            //gets profile record
            TProfile obj = null;

            try
            {
                return await _context.TProfiles.Where(p => p.ProfileId == profileId).FirstOrDefaultAsync();
            }
            catch (Exception x)
            {
                return obj;
            }
        }

        public async Task<IEnumerable<profileLookup>> getProfileLookupAsync()
        {
            //gets profile records using struct structure
            List<profileLookup> profiles = new List<profileLookup>();

            try
            {
                var dt = await _context.TProfiles.ToListAsync();

                if (dt != null)
                {
                    foreach(var d in dt)
                    {
                        var obj = new profileLookup() { id = d.ProfileId, nameOfProfile = d.ProfileName, companyId = (int) d.CompanyId };
                        profiles.Add(obj);
                    }
                }

                return profiles;
            }
            catch(Exception x)
            {
                return profiles;
            }
        }

        public async Task<bool> AmendUserModules(string _usr, string _newProfile)
        {
                try
                {
                    var pObj = await _context.TProfiles.Where(p => p.ProfileName == _newProfile).FirstOrDefaultAsync();
                    var u = await _context.Tusrs.Where(x => x.Usrname == _usr).FirstOrDefaultAsync();
                    if ((u != null) & (pObj != null))
                    {
                        //amend associated profile id
                        u.ProfileId = pObj.ProfileId;
                    }

                    await _context.SaveChangesAsync();

                    return true;
                }
                catch (Exception transErr)
                {
                    throw;
                }          
        }

        #endregion

        #region Authentication


        public async Task<Auth> GetUserAsync(App.POCOs.User userCredential)
        {
            Auth response = null;

            try
            {
                var user_info = await _context.Tusrs.Where(x => x.Usrname == userCredential.username).Where(x => x.Usrpassword == userCredential.password)
                    .Include(m => m.Profile)
                    .Include(m => m.Company)
                    .Include(m => m.Department)
                    .FirstOrDefaultAsync();

                if (user_info.UsrId > 0)
                {
                    response = new Auth()
                    {
                        status = true,
                        message = @"success",
                        user = new App.POCOs.UserData()
                        {
                            id = user_info.UsrId,
                            surname = user_info.Surname,
                            firstname = user_info.Firstname,
                            othernames = user_info.Othernames,
                            usrname = user_info.Usrname,
                            usrpassword = user_info.Usrpassword,
                            isAdmin = user_info.IsAdmin,
                            isLogged = user_info.IsLogged,
                            isActive = user_info.IsActive
                        },
                        company = new App.POCOs.Company()
                        {
                            id = user_info.Company.CompanyId,
                            company = user_info.Company.Company,
                            companyAddress = user_info.Company.CompanyAddress,
                            incorporationDate = user_info.Company.IncorporationDate
                        },
                        profile = new App.POCOs.Profile()
                        {
                            id = user_info.Profile.ProfileId,
                            profileString = user_info.Profile.ProfileString,
                            inUse = user_info.Profile.InUse,
                            dateAdded = user_info.Profile.DteAdded
                        },
                        department = new App.POCOs.Department()
                        {
                            id = user_info.Department.Id,
                            departmentName = user_info.Department.DepartmentName,
                            departmentDescription = user_info.Department.Describ
                        }
                    };

                    return response;
                }
                else
                {
                    return new Auth() { status = false, message = @"No data found" };
                }
            }
            catch (Exception x)
            {
                return new Auth()
                {
                    status = false,
                    message = $"{x.Message}"
                };
            }
        }

        public string HashPassword(SingleValue singleParam)
        {
            string result = string.Empty;

            try
            {
                var encryptionType = MD5.Create();
                byte[] data = encryptionType.ComputeHash(Encoding.UTF8.GetBytes(singleParam.stringValue));
                var encryptedString = string.Empty;
                for (int i = 0; i < data.Length; i++)
                {
                    encryptedString += data[i].ToString("x2").ToUpperInvariant();
                }

                return result = encryptedString;
            }
            catch (Exception x)
            {
                return result;
            }
        }

        #endregion

    }

    #region structs

    public struct moduleLookup
    {
        public int id { get; set; }
        public string module { get; set; }
        public string describ { get; set; }
    }

    public struct profileLookup
    {
        public int id { get; set; }
        public string nameOfProfile { get; set; }
        public int companyId { get; set; }
    }

    public struct userLookup
    {
        public int id { get; set; }
        public string usrname { get; set; }
        public string active { get; set; }
        public string logged { get; set; }
        public string isAdmin { get; set; }
        public string profile { get; set; }
    }

    #endregion

}
