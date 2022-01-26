using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinActiveTool
{
    public class AESHelper
    {
        /// <summary>
        /// 默认密钥-密钥的长度必须是32
        /// </summary>
        private const string PublicKey = "4569873210258741";

        /// <summary>
        /// 默认向量
        /// </summary>
        private const string Iv = "ajklbfghnocdimep";
        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="str">需要加密字符串</param>  
        /// <returns>加密后字符串</returns>  
        public static String Encrypt(string str)
        {
            return Encrypt(str, PublicKey);
        }

        public bool AesDecrypt(string str, DateTime dt, string key)
        {
            string strdate = "01122021";
            int day = dt.Day;
            int month = dt.Month;
            int yy1 = dt.Year / 100;
            int yy2 = dt.Year % 100;
            byte[] dateb = new byte[] { (byte)day, (byte)month, (byte)yy1 , (byte)yy2 };

            str = str + dt.ToString("ddMMyyyy");
            str = AESHelper.Encrypt(str);
            System.Text.RegularExpressions.Regex d = new System.Text.RegularExpressions.Regex(@"\W");
            str = d.Replace(str, "");
            string first = str.Substring(0, 12).ToUpper();
            string resultstr = first.Substring(0, 4) + "-" + first.Substring(4, 4) + "-" + first.Substring(8, 4);
            if (resultstr == key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="str">需要解密字符串</param>  
        /// <returns>解密后字符串</returns>  
        public static String Decrypt(string str)
        {
            return Decrypt(str, PublicKey);
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = System.Text.Encoding.UTF8.GetBytes(str);
            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <param name="key">32位密钥</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string str, string key)
        {
            Byte[] keyArray = System.Text.Encoding.UTF8.GetBytes(key);
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            var rijndael = new System.Security.Cryptography.RijndaelManaged();
            rijndael.Key = keyArray;
            rijndael.Mode = System.Security.Cryptography.CipherMode.ECB;
            rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            rijndael.IV = System.Text.Encoding.UTF8.GetBytes(Iv);
            System.Security.Cryptography.ICryptoTransform cTransform = rijndael.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return System.Text.Encoding.UTF8.GetString(resultArray);
        }
    }
}