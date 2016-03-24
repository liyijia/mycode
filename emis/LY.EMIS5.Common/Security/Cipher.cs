using LY.EMIS5.Common.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Security
{
    /// <summary>
    /// 密码
    /// </summary>
    public class Cipher
    {
        private static ICryptoTransform _RC2Decryptor;
        private static ICryptoTransform _RC2Encryptor;

        static Cipher()
        {
            if (_RC2Decryptor == null)
            {
                using (var rc2 = new RC2CryptoServiceProvider())
                {
                    rc2.GenerateIV();
                    rc2.GenerateKey();

                    _RC2Decryptor = rc2.CreateDecryptor();
                    _RC2Encryptor = rc2.CreateEncryptor();
                }
            }
        }

        /// <summary>
        /// 加密前或加密后的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 是否已经加密
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// 加密方式，也就是加密算法
        /// </summary>
        public SecurityModes SecurityMode { get; set; }

        /// <summary>
        /// 解密
        /// </summary>
        public Cipher Decrypt()
        {
            if (string.IsNullOrEmpty(this.Value))
            {
                this.IsEncrypted = false;
                return this;
            }
            if (!IsEncrypted)
                return this;
            if (SecurityMode == SecurityModes.MD5)
                throw new NotSupportedException("MD5加密方式是不可逆的，无法解密。");
            else
            {
                var input = this.Value.HexStringToByteArray();
                return new Cipher
                {
                    Value = Encoding.ASCII.GetString(_RC2Decryptor.TransformFinalBlock(input, 0, input.Length)),
                    IsEncrypted = false,
                    SecurityMode = this.SecurityMode
                };
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        public Cipher Encrypt()
        {
            if (IsEncrypted)
                return this;
            if (string.IsNullOrEmpty(this.Value))
            {
                this.IsEncrypted = true;
                return this;
            }
            var input = Encoding.ASCII.GetBytes(this.Value);
            var output = SecurityMode == SecurityModes.MD5 ?
                new MD5CryptoServiceProvider().ComputeHash(input).ByteArrayToHexString() :
                _RC2Encryptor.TransformFinalBlock(input, 0, input.Length).ByteArrayToHexString();
            return new Cipher
            {
                Value = output,
                IsEncrypted = true,
                SecurityMode = this.SecurityMode
            };
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var cipher = obj as Cipher;
            if (cipher == null || this.SecurityMode != cipher.SecurityMode)
                return false;
            return this.Encrypt().Value == cipher.Encrypt().Value;
        }

        public override int GetHashCode()
        {
            var cipher = this.Encrypt();
            return cipher.Value.GetHashCode() + cipher.IsEncrypted.GetHashCode() * 7 + cipher.SecurityMode.GetHashCode() * 17;
        }

        public static bool operator ==(Cipher cipher1, Cipher cipher2)
        {
            return cipher1.Equals(cipher2);
        }

        public static bool operator !=(Cipher cipher1, Cipher cipher2)
        {
            return !cipher1.Equals(cipher2);
        }
    }
}
