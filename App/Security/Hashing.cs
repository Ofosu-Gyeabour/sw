using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace App.Security
{
    public class Hashing
    {
        public static string CreateHash(string cipher)
        {
            //method encrypts a given password
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(cipher);
            data = x.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        public static bool MatchHash(string HashDB, string HashUser)
        {
            //method matches the hashed password from a database to the provided by the user
            HashUser = CreateHash(HashUser);
            if (HashUser == HashDB)
                return true;
            else
                return false;
        }

        public static string CreateMD5Hash(string cipher)
        {

            //using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            using (MD5 md5 = MD5.Create())
            {
                try
                {
                    byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(cipher);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("X2"));
                    }
                    return sb.ToString();
                }
                catch (Exception x)
                {
                    Debug.Print($"error:{x.Message}");
                    return String.Empty;
                }
            }
        }

        public static bool MatchMD5Hash(string HashFromDB, string HashFromUser)
        {
            HashFromUser = CreateHash(HashFromUser);
            if (HashFromUser == HashFromDB)
                return true;
            else
                return false;
        }


    }
}
