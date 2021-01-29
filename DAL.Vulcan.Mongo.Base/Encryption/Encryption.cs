using System;
using System.IO;
using System.Security.Cryptography;

namespace DAL.Vulcan.Mongo.Base.Encryption
{
    public class Encryption
    {
        public Encryption()
        {
        }

        public byte[] Key
        {
            get;
            set;
        }

        public byte[] IV
        {
            get;
            set;
        }

        public byte[] Encrypt<T>(T value)
        {
            if (value == null)
                return null;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(Key, IV);

                if (typeof(T) == typeof(byte[]))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            csEncrypt.Write(value as byte[], 0, (value as byte[]).Length);

                            if (!csEncrypt.HasFlushedFinalBlock)
                                csEncrypt.FlushFinalBlock();

                            return msEncrypt.ToArray();
                        }
                    }
                }
                else if (value is string)
                {
                    var str = value as string;
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(str);
                            }

                            if (!csEncrypt.HasFlushedFinalBlock)
                                csEncrypt.FlushFinalBlock();

                            return msEncrypt.ToArray();
                        }
                    }
                }
                else if (value is IConvertible)
                {
                    var str = Convert.ToString(value, new System.Globalization.CultureInfo("en-US", false));
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(str);
                            }

                            if (!csEncrypt.HasFlushedFinalBlock)
                                csEncrypt.FlushFinalBlock();

                            return msEncrypt.ToArray();
                        }
                    }
                }
                else
                {
                    throw new InvalidOperationException("Value is not a byte [] or IConvertable.");
                }
            }
        }

        public T Decrypt<T>(byte[] value)
        {
            if (value == null)
                return default(T);

            var typeOf = typeof(T).FullName;

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(Key, IV);

                using (MemoryStream msDecrypt = new MemoryStream(value as byte[]))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        if (typeof(T) == typeof(byte[]))
                        {
                            MemoryStream ms = new MemoryStream();

                            byte[] buffer = new byte[1024];
                            int length = csDecrypt.Read(buffer, 0, buffer.Length);
                            while (length > 0)
                            {
                                ms.Write(buffer, 0, length);

                                length = csDecrypt.Read(buffer, 0, buffer.Length);
                            }

                            ms.Close();

                            return (T)(object)ms.GetBuffer();
                        }
                        else if (typeof(T) == typeof(string))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                var str = srDecrypt.ReadToEnd();
                                return (T)(object)str;
                            }
                        }
                        else if (typeof(IConvertible).IsAssignableFrom(typeof(T)))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                var str = srDecrypt.ReadToEnd();
                                return (T)Convert.ChangeType(str, typeof(T), new System.Globalization.CultureInfo("en-US", false));
                            }
                        }
                        else if (Nullable.GetUnderlyingType(typeof(T)) != null && typeof(IConvertible).IsAssignableFrom(Nullable.GetUnderlyingType(typeof(T))))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                string str = srDecrypt.ReadToEnd();
                                return (T)Convert.ChangeType(str, Nullable.GetUnderlyingType(typeof(T)), new System.Globalization.CultureInfo("en-US", false));
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("T is not a byte [] or IConvertable or Nullable<IConvertable>.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a new Encryption with the default keys set.
        /// </summary>
        public static Encryption NewEncryption => new Encryption
        {
            Key = EncryptionKeys.Key,
            IV = EncryptionKeys.IV
        };
    }
}
