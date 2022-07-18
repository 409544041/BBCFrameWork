// ******************************************************************
//       /\ /|       @file       EncodeUtil
//       \ V/        @brief      加密工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2022-05-24 9:44
//    *(__\_\        @Copyright  Copyright (c) 2022, Shadowrabbit
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;
using UnityEngine;

namespace Rabi
{
    public static class EncodeUtil
    {
        #region MD5 加密

        /// <summary>
        /// MD5加密
        /// </summary>
        public static string Md532(this string source)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            var md5 = MD5.Create();
            return HashAlgorithmBase(md5, source, encoding);
        }

        /// <summary>
        /// 加盐MD5加密
        /// </summary>
        public static string Md532Salt(this string source, string salt)
        {
            return string.IsNullOrEmpty(salt) ? source.Md532() : (source + "『" + salt + "』").Md532();
        }

        #endregion

        #region SHA 加密

        /// <summary>
        /// SHA1 加密
        /// </summary>
        public static string Sha1(this string source)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return HashAlgorithmBase(sha1, source, encoding);
        }

        /// <summary>
        /// SHA256 加密
        /// </summary>
        public static string Sha256(this string source)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            SHA256 sha256 = new SHA256Managed();
            return HashAlgorithmBase(sha256, source, encoding);
        }

        /// <summary>
        /// SHA512 加密
        /// </summary>
        public static string Sha512(this string source)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            SHA512 sha512 = new SHA512Managed();
            return HashAlgorithmBase(sha512, source, encoding);
        }

        #endregion

        #region HMAC 加密

        /// <summary>
        /// HmacSha1 加密
        /// </summary>
        public static string HmacSha1(this string source, string keyVal)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            var keyStr = encoding.GetBytes(keyVal);
            var hmacSha1 = new HMACSHA1(keyStr);
            return HashAlgorithmBase(hmacSha1, source, encoding);
        }

        /// <summary>
        /// HmacSha256 加密
        /// </summary>
        public static string HmacSha256(this string source, string keyVal)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            var keyStr = encoding.GetBytes(keyVal);
            var hmacSha256 = new HMACSHA256(keyStr);
            return HashAlgorithmBase(hmacSha256, source, encoding);
        }

        /// <summary>
        /// HmacSha384 加密
        /// </summary>
        public static string HmacSha384(this string source, string keyVal)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            var keyStr = encoding.GetBytes(keyVal);
            var hmacSha384 = new HMACSHA384(keyStr);
            return HashAlgorithmBase(hmacSha384, source, encoding);
        }

        /// <summary>
        /// HmacSha512 加密
        /// </summary>
        public static string HmacSha512(this string source, string keyVal)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            var keyStr = encoding.GetBytes(keyVal);
            var hmacSha512 = new HMACSHA512(keyStr);
            return HashAlgorithmBase(hmacSha512, source, encoding);
        }

        /// <summary>
        /// HmacMd5 加密
        /// </summary>
        public static string HmacMd5(this string source, string keyVal)
        {
            if (string.IsNullOrEmpty(source)) return null;
            var encoding = Encoding.UTF8;
            var keyStr = encoding.GetBytes(keyVal);
            var hmacMd5 = new HMACMD5(keyStr);
            return HashAlgorithmBase(hmacMd5, source, encoding);
        }

        #endregion

        #region AES 加密解密

        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="source">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <returns></returns>  
        public static string AesStr(this string source, string keyVal)
        {
            if (keyVal == null || keyVal.Length != 16)
            {
                Debug.LogError("秘钥必须为16个字符");
                return source;
            }

            var ivVal = keyVal; //加密辅助向量
            var encoding = Encoding.UTF8;
            var btKey = keyVal.FormatByte(encoding);
            var btIv = ivVal.FormatByte(encoding);
            var byteArray = encoding.GetBytes(source);
            string encrypt;
            var aes = Rijndael.Create();
            using (var mStream = new MemoryStream())
            {
                using (var cStream =
                    new CryptoStream(mStream, aes.CreateEncryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }

            aes.Clear();
            return encrypt;
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="source">待加密字段</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <returns></returns>  
        public static string UnAesStr(this string source, string keyVal)
        {
            if (keyVal == null || keyVal.Length != 16)
            {
                Debug.LogError("秘钥必须为16个字符");
                return source;
            }

            var ivVal = keyVal; //加密辅助向量
            var encoding = Encoding.UTF8;
            var btKey = keyVal.FormatByte(encoding);
            var btIv = ivVal.FormatByte(encoding);
            var byteArray = Convert.FromBase64String(source);
            string decrypt;
            var aes = Rijndael.Create();
            using (var mStream = new MemoryStream())
            {
                using (var cStream =
                    new CryptoStream(mStream, aes.CreateDecryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = encoding.GetString(mStream.ToArray());
                }
            }

            aes.Clear();
            return decrypt;
        }

        /// <summary>  
        /// AES Byte类型 加密  
        /// </summary>  
        /// <param name="data">待加密明文</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <returns></returns>  
        public static byte[] AesByte(this byte[] data, string keyVal)
        {
            if (keyVal == null || keyVal.Length != 16)
            {
                Debug.LogError("秘钥必须为16个字符");
                return data;
            }

            var ivVal = keyVal;
            var bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(bKey.Length)), bKey, bKey.Length);
            var bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] cryptograph;
            var aes = Rijndael.Create();
            try
            {
                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bVector),
                        CryptoStreamMode.Write))
                    {
                        cStream.Write(data, 0, data.Length);
                        cStream.FlushFinalBlock();
                        cryptograph = mStream.ToArray();
                    }
                }
            }
            catch
            {
                cryptograph = null;
            }

            return cryptograph;
        }

        /// <summary>  
        /// AES Byte类型 解密  
        /// </summary>  
        /// <param name="data">待解密明文</param>  
        /// <param name="keyVal">密钥值</param>  
        /// <returns></returns>  
        public static byte[] UnAesByte(this byte[] data, string keyVal)
        {
            if (keyVal == null || keyVal.Length != 16)
            {
                Debug.LogError("秘钥必须为16个字符");
                return data;
            }

            var ivVal = keyVal;
            var bKey = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(keyVal.PadRight(bKey.Length)), bKey, bKey.Length);
            var bVector = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(ivVal.PadRight(bVector.Length)), bVector, bVector.Length);
            byte[] original;
            var aes = Rijndael.Create();
            try
            {
                using (var mStream = new MemoryStream(data))
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bVector),
                        CryptoStreamMode.Read))
                    {
                        using (var originalMemory = new MemoryStream())
                        {
                            var buffer = new byte[1024];
                            int readBytes;
                            while ((readBytes = cStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                originalMemory.Write(buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }

            return original;
        }

        #endregion

        #region RSA 加密解密

        //密钥对，请配合密钥生成工具使用『 http://download.csdn.net/detail/downiis6/9464639 』
        private const string PublicRsaKey = @"pubKey";
        private const string PrivateRsaKey = @"priKey";

        /// <summary>
        /// RSA 加密
        /// </summary>
        public static string Rsa(this string source)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PublicRsaKey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(source), true);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        public static string UnRsa(this string source)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(PrivateRsaKey);
            var cipherbytes = rsa.Decrypt(Convert.FromBase64String(source), true);
            return Encoding.UTF8.GetString(cipherbytes);
        }

        #endregion

        #region DES 加密解密

        /// <summary>
        /// DES 加密
        /// </summary>
        public static string Des(this string source, string keyVal, string ivVal)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(source);
                var des = new DESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal),
                    IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal)
                };
                var desencrypt = des.CreateEncryptor();
                var result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return BitConverter.ToString(result);
            }
            catch
            {
                Debug.LogError("DES转换出错");
                return string.Empty;
            }
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        public static string UnDes(this string source, string keyVal, string ivVal)
        {
            try
            {
                var sInput = source.Split("-".ToCharArray());
                var data = new byte[sInput.Length];
                for (var i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }

                var des = new DESCryptoServiceProvider
                {
                    Key = Encoding.ASCII.GetBytes(keyVal.Length > 8 ? keyVal.Substring(0, 8) : keyVal),
                    IV = Encoding.ASCII.GetBytes(ivVal.Length > 8 ? ivVal.Substring(0, 8) : ivVal)
                };
                var desencrypt = des.CreateDecryptor();
                var result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
            catch
            {
                Debug.LogError("DES解密出错");
                return string.Empty;
            }
        }

        #endregion

        #region TripleDES 加密解密

        /// <summary>
        /// DES3 加密
        /// </summary>
        public static string Des3(this string source, string keyVal)
        {
            try
            {
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = keyVal.FormatByte(Encoding.UTF8),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                using (var ms = new MemoryStream())
                {
                    var btArray = Encoding.UTF8.GetBytes(source);
                    try
                    {
                        using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(btArray, 0, btArray.Length);
                            cs.FlushFinalBlock();
                        }

                        return ms.ToArray().Bytes2Str();
                    }
                    catch
                    {
                        return source;
                    }
                }
            }
            catch
            {
                Debug.LogError("TripleDES加密出现错误");
                return string.Empty;
            }
        }

        /// <summary>
        /// DES3 解密
        /// </summary>
        public static string UnDes3(this string source, string keyVal)
        {
            try
            {
                var byArray = source.Str2Bytes();
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = keyVal.FormatByte(Encoding.UTF8),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(byArray, 0, byArray.Length);
                        cs.FlushFinalBlock();
                        cs.Close();
                        ms.Close();
                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch
            {
                Debug.LogError("TripleDES解密出现错误");
                return string.Empty;
            }
        }

        #endregion

        #region BASE64 加密解密

        /// <summary>
        /// BASE64 加密
        /// </summary>
        /// <param name="source">待加密字段</param>
        /// <returns></returns>
        public static string Base64(this string source)
        {
            var btArray = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(btArray, 0, btArray.Length);
        }

        /// <summary>
        /// BASE64 解密
        /// </summary>
        /// <param name="source">待解密字段</param>
        /// <returns></returns>
        public static string UnBase64(this string source)
        {
            var btArray = Convert.FromBase64String(source);
            return Encoding.UTF8.GetString(btArray);
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 转成数组
        /// </summary>
        private static byte[] Str2Bytes(this string source)
        {
            source = source.Replace(" ", "");
            var buffer = new byte[source.Length / 2];
            for (var i = 0; i < source.Length; i += 2) buffer[i / 2] = Convert.ToByte(source.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        private static string Bytes2Str(this IEnumerable<byte> source, string formatStr = "{0:X2}")
        {
            var pwd = new StringBuilder();
            foreach (var btStr in source)
            {
                pwd.AppendFormat(formatStr, btStr);
            }

            return pwd.ToString();
        }

        private static byte[] FormatByte(this string strVal, Encoding encoding)
        {
            return encoding.GetBytes(strVal.Base64().Substring(0, 16).ToUpper());
        }

        /// <summary>
        /// HashAlgorithm 加密统一方法
        /// </summary>
        private static string HashAlgorithmBase(HashAlgorithm hashAlgorithmObj, string source, Encoding encoding)
        {
            var btStr = encoding.GetBytes(source);
            var hashStr = hashAlgorithmObj.ComputeHash(btStr);
            return hashStr.Bytes2Str();
        }

        #endregion
    }
}