using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace acl
{
    public static class ACLs
    {
        public  static string genSecToken()
        {
            string secToken5mins = "";
            string macName = Environment.MachineName;
            string userID = Environment.UserName;
            DateTime forENC = DateTime.Now;
            string unEnc = macName + "!" + userID + "!" + forENC;
            byte[] uncText = Encoding.Unicode.GetBytes(unEnc);
            string EncryptKey = "cresrocks";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pik = new Rfc2898DeriveBytes(EncryptKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pik.GetBytes(32);
                encryptor.IV = pik.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(uncText, 0, uncText.Length);
                        cs.Close();
                    }
                    secToken5mins = Convert.ToBase64String(ms.ToArray());
                }
            }


            return secToken5mins;
        }

        public static byte[] decryptedFile(byte[] encFile, string EncryptionKey = "")
        {
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encFile, 0, encFile.Length);
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }

        }
    }
}
