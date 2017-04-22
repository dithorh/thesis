using System.IO;
using System.Security.Cryptography;
using System;
using System.Text;

namespace EnCryptDecrypt
{
    class TDES
    {
        //  Call this function to remove the key from memory after use for security
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        public static string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            TripleDESCryptoServiceProvider tdesCrypto = (TripleDESCryptoServiceProvider)TripleDESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption.
            return Encoding.ASCII.GetString(tdesCrypto.Key);
        }

        public static void EncryptFile(string inputFile, string outputFile, string key)
        {
            try
            {
                string[] split = outputFile.Split('.');
                outputFile = split[0] + "_enc." + split[1];

                FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
                FileStream fsEncrypted = new FileStream(outputFile, FileMode.Create, FileAccess.Write);

                TripleDESCryptoServiceProvider TDES = new TripleDESCryptoServiceProvider();
                TDES.Key = Encoding.ASCII.GetBytes(key);
                //TDES.IV = Encoding.ASCII.GetBytes(key);
                ICryptoTransform tdesEncrypt = TDES.CreateEncryptor();
                //CryptoStream cryptostream = new CryptoStream(fsEncrypted, new TripleDESCryptoServiceProvider().CreateEncryptor(TDES.Key, TDES.IV), CryptoStreamMode.Write);
                CryptoStream cryptostream = new CryptoStream(fsEncrypted, tdesEncrypt, CryptoStreamMode.Write);

                byte[] buffer = new byte[fsInput.Length];
                fsInput.Read(buffer, 0, buffer.Length);
                cryptostream.Write(buffer, 0, buffer.Length);
                cryptostream.FlushFinalBlock();
                cryptostream.Close();
                fsInput.Close();
                fsEncrypted.Close();
            }
            catch (CryptographicException ce)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", ce.Message);
            }
            catch (UnauthorizedAccessException uae)
            {
                Console.WriteLine("A file access error occurred: {0}", uae.Message);
            }
        }

        public static void DecryptFile(string inputFile, string outputFile, string key)
        {
            string[] split = outputFile.Split('.');
            outputFile = split[0] + "_dec." + split[1];

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.Default.GetBytes(key));
            hashmd5.Clear();

            using (var TDES = new TripleDESCryptoServiceProvider())
            {
                TDES.Key = keyArray;

                using (var tdesdecrypt = TDES.CreateDecryptor())
                {
                    using (var fsread = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                    {
                        using (var cryptostreamDecr = new CryptoStream(fsread, tdesdecrypt, CryptoStreamMode.Read))
                        {
                            using (var fswrite = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                            {
                                cryptostreamDecr.CopyTo(fswrite);
                            }
                        }
                    }
                }
            }
            //try
            //{
            //    byte[] keyArray;

            //    string[] split = outputFile.Split('.');
            //    outputFile = split[0] + "_dec." + split[1];

            //    FileStream fsRead = new FileStream(inputFile, FileMode.Open, FileAccess.Read);

            //    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            //    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //    hashmd5.Clear();

            //    TripleDESCryptoServiceProvider TDES = new TripleDESCryptoServiceProvider();
            //    TDES.Key = keyArray;
            //    ICryptoTransform tdesdecrypt = TDES.CreateDecryptor();
            //    //CryptoStream cryptostreamdec = new CryptoStream(fsRead, new TripleDESCryptoServiceProvider().CreateDecryptor(TDES.Key, TDES.IV), CryptoStreamMode.Read);
            //    CryptoStream cryptostreamdec = new CryptoStream(fsRead, tdesdecrypt, CryptoStreamMode.Read);

            //    StreamWriter fsDecrypted = new StreamWriter(outputFile);
            //    fsDecrypted.Write(new StreamReader(cryptostreamdec).ReadToEnd());
            //    fsDecrypted.Flush();
            //    fsDecrypted.Close();
            //}
            //catch (CryptographicException ce)
            //{
            //    Console.WriteLine("A Cryptographic error occurred: {0}", ce.Message);
            //}
            //catch (UnauthorizedAccessException uae)
            //{
            //    Console.WriteLine("A file access error occurred: {0}", uae.Message);
            //}
        }
    }
}
