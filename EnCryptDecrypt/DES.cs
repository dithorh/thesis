using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;

namespace EnCryptDecrypt
{
    class DES
    {
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        // Function to Generate a 64 bits Key.
        public static string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption.
            return Encoding.ASCII.GetString(desCrypto.Key);
        }

        public static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            string[] split = sOutputFilename.Split('.');
            sOutputFilename = split[0] + "_enc." + split[1];

            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(sKey);

                    /* This is for demostrating purposes only. 
                     * Ideally you will want the IV key to be different from your key and you should always generate a new one for each encryption in other to achieve maximum security*/
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(sKey);

                    using (FileStream fsCrypt = new FileStream(sOutputFilename, FileMode.Create))
                    {
                        using (ICryptoTransform encryptor = aes.CreateEncryptor(key, IV))
                        {
                            using (CryptoStream cs = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (FileStream fsIn = new FileStream(sInputFilename, FileMode.Open))
                                {
                                    int data;
                                    while ((data = fsIn.ReadByte()) != -1)
                                    {
                                        cs.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // failed to encrypt file
            }

            //FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            //FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

            //DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //DES.Key = Encoding.ASCII.GetBytes(sKey);
            //DES.IV = Encoding.ASCII.GetBytes(sKey);
            //ICryptoTransform desencrypt = DES.CreateEncryptor();
            //CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

            //byte[] bytearrayinput = new byte[fsInput.Length];
            //fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            //cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            //cryptostream.Close();
            //fsInput.Close();
            //fsEncrypted.Close();
        }

        public static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            string[] split = sOutputFilename.Split('.');
            sOutputFilename = split[0] + "_dec." + split[1];

            try
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    byte[] key = ASCIIEncoding.UTF8.GetBytes(sKey);

                    /* This is for demostrating purposes only. 
                     * Ideally you will want the IV key to be different from your key and you should always generate a new one for each encryption in other to achieve maximum security*/
                    byte[] IV = ASCIIEncoding.UTF8.GetBytes(sKey);

                    using (FileStream fsCrypt = new FileStream(sInputFilename, FileMode.Open))
                    {
                        using (FileStream fsOut = new FileStream(sOutputFilename, FileMode.Create))
                        {
                            using (ICryptoTransform decryptor = aes.CreateDecryptor(key, IV))
                            {
                                using (CryptoStream cs = new CryptoStream(fsCrypt, decryptor, CryptoStreamMode.Read))
                                {
                                    int data;
                                    while ((data = cs.ReadByte()) != -1)
                                    {
                                        fsOut.WriteByte((byte)data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // failed to decrypt file
            }

            //using (var DES = new DESCryptoServiceProvider())
            //{
            //    DES.Key = Encoding.ASCII.GetBytes(sKey);
            //    DES.IV = Encoding.ASCII.GetBytes(sKey);
            //    using (var desdecrypt = DES.CreateDecryptor())
            //    {
            //        using (var fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read))
            //        {
            //            using (var cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read))
            //            {
            //                using (var fswrite = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write))
            //                {
            //                    cryptostreamDecr.CopyTo(fswrite);
            //                }
            //            }
            //        }
            //    }
            //}

            //DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            ////A 64 bit key and IV is required for this provider.
            ////Set secret key For DES algorithm.
            //DES.Key = Encoding.UTF8.GetBytes(sKey);
            ////Set initialization vector.
            //DES.IV = Encoding.UTF8.GetBytes(sKey);
            //DES.Padding = PaddingMode.None;

            ////Create a file stream to read the encrypted file back.
            //FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            ////Create a DES decryptor from the DES instance.
            //ICryptoTransform desdecrypt = DES.CreateDecryptor();
            ////Create crypto stream set to read and do a
            ////DES decryption transform on incoming bytes.
            //CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            ////Print the contents of the decrypted file.
            //StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            //fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            //fsDecrypted.Flush();
            //fsDecrypted.Close();
        }
    }
}
