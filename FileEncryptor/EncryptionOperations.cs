using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileEncryptor
{
    internal class Encryptor
    {
        public static byte[] ConvertToByteArray(string value)
        {
            byte[] bytes = new byte[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                bytes[i] = (byte)value[i];
            }
            return bytes;
        }
        public static void EncryptFile(string path, byte[] key)
        {
            string tmpPath = Path.GetTempFileName();
            using (FileStream fsSrc = File.OpenRead(path))
            using (AesManaged aes = new AesManaged() { Key = key })
            using (FileStream fsDst = File.Create(tmpPath))
            {
                fsDst.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new CryptoStream(fsDst, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
                {
                    fsSrc.CopyTo(cs);
                }
            }
            File.Delete(path);
            File.Move(tmpPath, path);
        }
        public static void DecryptFile(string path, byte[] key)
        {
            string tmpPath = Path.GetTempFileName();
            using (FileStream fsSrc = File.OpenRead(path))
            {
                byte[] iv = new byte[16];
                fsSrc.Read(iv, 0, iv.Length);
                using (AesManaged aes = new AesManaged() { Key = key, IV = iv })
                using (CryptoStream cs = new CryptoStream(fsSrc, aes.CreateDecryptor(), CryptoStreamMode.Read, true))
                using (FileStream fsDst = File.Create(tmpPath))
                {
                    cs.CopyTo(fsDst);
                }
            }
            File.Delete(path);
            File.Move(tmpPath, path);
        }
    }
}
