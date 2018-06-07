using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Universal.Core
{
    public class EncryptorHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string GetMD5(string sourceString)
        {
            MD5 md5 = MD5.Create();
            byte[] source= md5.ComputeHash(Encoding.UTF8.GetBytes(sourceString));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < source.Length; i++)
            {
                sb.Append(source[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取真随机数盐值
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string CreateSaltKey(int size = 32)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }
    }
}
