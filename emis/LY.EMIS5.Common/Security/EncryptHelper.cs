using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Security
{
    /// <summary>
    /// 加密解密帮助类
    /// </summary>
    public class EncryptHelper
    {
        #region Base64加密解密

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字符串</param>
        /// <param name="encode">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input)
        {
            return Base64Decrypt(input, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="input">需要解密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input, Encoding encode)
        {
            return encode.GetString(Convert.FromBase64String(input));
        }

        #endregion

        #region MD5加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encryptString">待加密的密文</param>
        /// <returns></returns>
        public static string MD5Encrypt(string encryptString)
        {
            string returnValue;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                returnValue = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(encryptString))).Replace("-", "");
                md5.Clear();
            }
            return returnValue;
        }

        #endregion

        #region DES加密解密

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">待加密的密文</param>
        /// <param name="encryptKey">密匙（8位）</param>
        /// <returns></returns>
        public static string DESEncrypt(string encryptString, string encryptKey)
        {
            string returnValue;
            try
            {
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] byteEncrypt = Encoding.UTF8.GetBytes(encryptString);

                using (DESCryptoServiceProvider dES = new DESCryptoServiceProvider())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, dES.CreateEncryptor(Encoding.UTF8.GetBytes(encryptKey), rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(byteEncrypt, 0, byteEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    returnValue = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString">密文</param>
        /// <param name="decryptKey">密匙（8位）</param>
        /// <returns></returns>
        public static string DESDecrypt(string decryptString, string decryptKey)
        {
            string returnValue;
            try
            {
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] byteDecryptString = Convert.FromBase64String(decryptString);

                using (DESCryptoServiceProvider dES = new DESCryptoServiceProvider())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, dES.CreateDecryptor(Encoding.UTF8.GetBytes(decryptKey), rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(byteDecryptString, 0, byteDecryptString.Length);
                    cryptoStream.FlushFinalBlock();

                    returnValue = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }
            return returnValue;

        }

        #endregion

        #region RC2加密解密

        /// <summary>
        /// RC2加密
        /// </summary>
        /// <param name="encryptString">待加密的密文</param>
        /// <param name="encryptKey">密匙(必须为5-16位)</param>
        /// <returns></returns>
        public static string RC2Encrypt(string encryptString, string encryptKey)
        {
            string returnValue;
            try
            {
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] byteEncryptString = Encoding.UTF8.GetBytes(encryptString);

                using (RC2CryptoServiceProvider rC2 = new RC2CryptoServiceProvider())
                {
                    MemoryStream memorystream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memorystream, rC2.CreateEncryptor(Encoding.UTF8.GetBytes(encryptKey), rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(byteEncryptString, 0, byteEncryptString.Length);
                    cryptoStream.FlushFinalBlock();

                    returnValue = Convert.ToBase64String(memorystream.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// RC2解密
        /// </summary>
        /// <param name="decryptString">密文</param>
        /// <param name="decryptKey">密匙(必须为5-16位)</param>
        /// <returns></returns>
        public static string RC2Decrypt(string decryptString, string decryptKey)
        {
            string returnValue;
            try
            {
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                byte[] byteDecrytString = Convert.FromBase64String(decryptString);

                using (RC2CryptoServiceProvider rC2 = new RC2CryptoServiceProvider())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, rC2.CreateDecryptor(Encoding.UTF8.GetBytes(decryptKey), rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(byteDecrytString, 0, byteDecrytString.Length);
                    cryptoStream.FlushFinalBlock();

                    returnValue = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }

        #endregion

        #region 3DES 加密解密

        /// <summary>
        /// 3DES 加密
        /// </summary>
        /// <param name="encryptString">待加密密文</param>
        /// <param name="encryptKey1">密匙1(长度必须为8位)</param>
        /// <param name="encryptKey2">密匙2(长度必须为8位)</param>
        /// <param name="encryptKey3">密匙3(长度必须为8位)</param>
        /// <returns></returns>
        public static string DES3Encrypt(string encryptString, string encryptKey1, string encryptKey2, string encryptKey3)
        {
            string returnValue;
            try
            {
                returnValue = DESEncrypt(encryptString, encryptKey3);
                returnValue = DESEncrypt(returnValue, encryptKey2);
                returnValue = DESEncrypt(returnValue, encryptKey1);
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// 3DES 解密
        /// </summary>
        /// <param name="decryptString">待解密密文</param>
        /// <param name="decryptKey1">密匙1(长度必须为8位)</param>
        /// <param name="decryptKey2">密匙2(长度必须为8位)</param>
        /// <param name="decryptKey3">密匙3(长度必须为8位)</param>
        /// <returns></returns>
        public static string DES3Decrypt(string decryptString, string decryptKey1, string decryptKey2, string decryptKey3)
        {
            string returnValue = string.Empty;
            try
            {
                returnValue = DESDecrypt(decryptString, decryptKey1);
                returnValue = DESDecrypt(returnValue, decryptKey2);
                returnValue = DESDecrypt(returnValue, decryptKey3);
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }

        #endregion

        #region AES加密解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptString">待加密的密文</param>
        /// <param name="encryptKey">加密密匙</param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptString, string encryptKey)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] rgbIV = Convert.FromBase64String("Y3F5cTIwMTRAKSEkYWJjZA==");
                byte[] byteEncryptString = Encoding.UTF8.GetBytes(encryptString);

                using (Rijndael AESProvider = Rijndael.Create())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, AESProvider.CreateEncryptor(Encoding.UTF8.GetBytes(encryptKey), rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(byteEncryptString, 0, byteEncryptString.Length);
                    cryptoStream.FlushFinalBlock();

                    returnValue = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }

        /// <summary>
        ///AES 解密
        /// </summary>
        /// <param name="decryptString">待解密密文</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(string decryptString, string decryptKey)
        {
            string returnValue = string.Empty;
            try
            {
                byte[] rgbIV = Convert.FromBase64String("Y3F5cTIwMTRAKSEkYWJjZA==");
                byte[] byteDecryptString = Convert.FromBase64String(decryptString);

                using (Rijndael AESProvider = Rijndael.Create())
                {
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, AESProvider.CreateDecryptor(Encoding.UTF8.GetBytes(decryptKey), rgbIV), CryptoStreamMode.Write);
                    cryptoStream.Write(byteDecryptString, 0, byteDecryptString.Length);
                    cryptoStream.FlushFinalBlock();

                    returnValue = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return returnValue;
        }

        #endregion
    }
}
