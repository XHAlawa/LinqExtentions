using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LinqExtensions
{
    public static class StringExtensionMethods
    {
        public static string Encrypt(this string input, string key)
        {
            Exceptions.ThrowIfNull(input, nameof(input));
            Exceptions.ThrowIfNull(key, nameof(key));
            
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
#pragma warning disable SYSLIB0021,S5547 
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = UTF8Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
#pragma warning restore SYSLIB0021,S5547
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(this string input, string key)
        {
            Exceptions.ThrowIfNull(input, nameof(input));
            Exceptions.ThrowIfNull(key, nameof(key));
            
            byte[] inputArray = Convert.FromBase64String(input);
#pragma warning disable SYSLIB0021,S5547
            var tripleDES = new TripleDESCryptoServiceProvider
            {
                Key = UTF8Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
#pragma warning restore SYSLIB0021
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
