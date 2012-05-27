namespace Itransition.Course.WebReg.Modules.Extensions
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class StringHashExtensions
    {
        public static String HashMD5(this String str)
        {
            var ue = new UnicodeEncoding();
            var message = ue.GetBytes(str);

            var hashString = new MD5CryptoServiceProvider();
            var hexBuilder = new StringBuilder();

            var hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hexBuilder.Append(String.Format("{0:X2}", x));
            }
            return hexBuilder.ToString();            
        }
    }
}
