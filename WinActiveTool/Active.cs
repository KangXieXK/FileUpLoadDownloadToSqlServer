using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
namespace WinActiveTool
{
    public class Active
    {

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public string AesEncrypt(string str, DateTime dt)
        {
            //gmhp123401F47
            int daycount = (int)(dt - new DateTime(2000, 1, 1)).TotalDays;
            string DateString = daycount.ToString("X").PadLeft(5, '0');

            str = str + DateString;
            str = AESHelper.Encrypt(str);
            System.Text.RegularExpressions.Regex d = new System.Text.RegularExpressions.Regex(@"\W");
            str = d.Replace(str, "");
            string first = str.Substring(0, 15).ToUpper();
            string resultstr = DateString + "-" + first.Substring(0, 5) + "-" + first.Substring(5, 5)+"-" + first.Substring(10, 5);
            return resultstr.ToUpper();
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public bool AesDecrypt(string str, string key, out DateTime day)
        {
            //gmhp1234 01F47-aaaaa-bbbbb
            int daycount = Convert.ToInt32(key.Split('-')[0], 16);
            day = new DateTime(2000, 1, 1).AddDays(daycount);
            str = str + key.Split('-')[0];
            str = AESHelper.Encrypt(str);
            System.Text.RegularExpressions.Regex d = new System.Text.RegularExpressions.Regex(@"\W");
            str = d.Replace(str, "");
            string first = str.Substring(0, 15).ToUpper();
            string resultstr = key.Split('-')[0] + "-" + first.Substring(0, 5) + "-" + first.Substring(5, 5) + "-" + first.Substring(10, 5);
            if (resultstr == key)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        ///// <summary>
        /////  AES 加密
        ///// </summary>
        ///// <param name="str">明文（待加密）</param>
        ///// <param name="key">密文</param>
        ///// <returns></returns>
        //public string AesEncrypt(string str, string key)
        //{
        //    if (string.IsNullOrEmpty(str)) return null;
        //    Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

        //    RijndaelManaged rm = new RijndaelManaged
        //    {
        //        Key = Encoding.UTF8.GetBytes(key),
        //        Mode = CipherMode.ECB,
        //        Padding = PaddingMode.PKCS7
        //    };

        //    ICryptoTransform cTransform = rm.CreateEncryptor();
        //    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        //    return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        //}
        ///// <summary>
        /////  AES 解密
        ///// </summary>
        ///// <param name="str">明文（待解密）</param>
        ///// <param name="key">密文</param>
        ///// <returns></returns>
        //public string AesDecrypt(string str, string key)
        //{
        //    if (string.IsNullOrEmpty(str)) return null;
        //    Byte[] toEncryptArray = Convert.FromBase64String(str);

        //    RijndaelManaged rm = new RijndaelManaged
        //    {
        //        Key = Encoding.UTF8.GetBytes(key),
        //        Mode = CipherMode.ECB,
        //        Padding = PaddingMode.PKCS7
        //    };

        //    ICryptoTransform cTransform = rm.CreateDecryptor();
        //    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        //    return Encoding.UTF8.GetString(resultArray);

        //}
    }
}
