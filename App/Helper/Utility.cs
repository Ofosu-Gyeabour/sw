using App.POCOs;

namespace App.Helper
{
    public class Utility
    {
        swContext _context;
        public Utility() {
            _context = new swContext();
        }

        public async Task<User> getUserRecordAsync(string _usrname)
        {
            //method gets a record of the user and the associated password
            User result = null;

            try
            {
                var dta = await _context.Tusrs.Where(x => x.Usrname == _usrname).FirstOrDefaultAsync();
                if (dta != null)
                {
                    result = new User()
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

    }
}
