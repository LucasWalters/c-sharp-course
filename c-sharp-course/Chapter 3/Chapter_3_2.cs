using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace C_sharp_Course.Chapter_3
{
    class Chapter_3_2
    {
        // Used in AES Encryption/decryption
        static byte[] key;
        static byte[] initializationVector;
        static byte[] encryptedText;

        public static void Run()
        {
           /* Console.WriteLine("AES Encryption");
            AESEncryption();

            Console.WriteLine("\n\n\nAES Decryption");
            AESDecryption();*/

            // Note: different from book.
            // The RSA method in the book (page 244) does not work due to .NET core mismatch.
           // Console.WriteLine("\n\nRSA Encryption and Decryption");
            //RSAEncryptDecryptDemo();

            // Broken due to .NET Core mismatch
            //Console.WriteLine("\n\n[Broken]RSA KeyStore demo");
           // RSAKeyStore();

            // Broken. TODO: debug (maybe..)
            //Console.WriteLine("\n\n[Broken]Signing data");
            //RSASignData();

            //Console.WriteLine("\n\nSimple Checksum Hashing");
            SimpleCheckSum("Hello World");

            //Console.WriteLine("\n\nSHA2 Hashing");
            SHA2();

            //Console.WriteLine("\n\n\nDouble AES Encryption");
            DoubleAESEncryption();

            Console.WriteLine("\n\n ---");
        }

        private static void AESEncryption()
        {
            string plainText = "Snape kills Dumbledore";
            Console.WriteLine($"Text to encrypt: {plainText}");

            using (Aes aes = Aes.Create())
            {
                // Grabs generated key and IV from AES
                key = aes.Key;
                initializationVector = aes.IV;

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (MemoryStream encryptMemoryStream = new MemoryStream())
                {
                    using (CryptoStream encryptCryptoStream =
                        new CryptoStream(encryptMemoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriterEncrypt = new StreamWriter(encryptCryptoStream))
                        {
                            streamWriterEncrypt.Write(plainText);
                        }
                        encryptedText = encryptMemoryStream.ToArray();
                    }
                }
            }

            Console.WriteLine("\nKey used to encrypt:");
            foreach (Byte b in key)
            {
                Console.Write(b);
            }
            Console.WriteLine("\n\nIV used to have random starting place:");
            foreach (Byte b in initializationVector)
            {
                Console.Write(b);
            }
            Console.WriteLine("\n\nEncrypted message:");
            foreach (Byte b in encryptedText)
            {
                Console.Write(b);
            }
        }

        private static void AESDecryption()
        {
            string decryptedText;
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = initializationVector;

                ICryptoTransform decryptor = aes.CreateDecryptor();
                using (MemoryStream decryptStream = new MemoryStream(encryptedText))
                {
                    using (CryptoStream decryptCryptoStream =
                        new CryptoStream(decryptStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReaderDecrypt = new StreamReader(decryptCryptoStream))
                        {
                            decryptedText = streamReaderDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            Console.WriteLine($"Decrypted text: {decryptedText}");
        }

        private static void RSAEncryptDecryptDemo()
        {
            string plainText = "Secret stuff";

            // RSA works on byte arrays, not strings
            ASCIIEncoding converter = new ASCIIEncoding();
            byte[] plainBytes = converter.GetBytes(plainText);
            byte[] encryptedBytes;
            byte[] decryptedBytes;

            using (RSACryptoServiceProvider rsaEncrypt = new RSACryptoServiceProvider())
            {
                encryptedBytes = RSAEncryptDecrypt.RSAEncrypt(plainBytes, rsaEncrypt.ExportParameters(false), false);
                Console.WriteLine($"Encrypted message: {converter.GetString(encryptedBytes)}");
                decryptedBytes = RSAEncryptDecrypt.RSADecrypt(encryptedBytes, rsaEncrypt.ExportParameters(true), false);
                Console.WriteLine($"\n\nDecrypted message: {converter.GetString(decryptedBytes)}");

               // Console.WriteLine(rsaEncrypt.ToXmlString(false)); // Public key BROKEN
                // Console.WriteLine(rsaEncrypt.ToXmlString(true)); // Private key BROKEN
            }
        }

        private static void RSAKeyStore()
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = "KeyStore";
            using (RSACryptoServiceProvider rsaStore = new RSACryptoServiceProvider(csp))
            {
                Console.WriteLine($"Stored keys: {rsaStore.ToXmlString(true)}");
                Console.WriteLine($"Loaded keys: {rsaStore.ToXmlString(false)}");
            }
        }

        private static void RSASignData()
        {
            ASCIIEncoding converter = new ASCIIEncoding();
            using (X509Store store = new X509Store("demoCertStore", StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                using (X509Certificate2 cert = store.Certificates[0])
                {
                    Console.WriteLine($"Cert: {cert.PrivateKey.KeySize}");
                    using (RSACryptoServiceProvider encryptProvider = cert.PrivateKey as RSACryptoServiceProvider)
                    {
                        Console.WriteLine($"Provider: {encryptProvider}");

                        string msgToSign = "I want to sign with this message";
                        byte[] msgToSignBytes = converter.GetBytes(msgToSign);

                        using (HashAlgorithm hasher = new SHA1Managed())
                        {
                            byte[] hash = hasher.ComputeHash(msgToSignBytes);
                            foreach (byte b in hash)
                            {
                                Console.WriteLine(b);
                            }
                           // byte[] signature = encryptProvider.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
                        }
                    }
                }
            }
        }

        private static void SimpleCheckSum(string plainText)
        {
            // Note: this does not guarantee unique checksums and is not a feasible solution
            int total = 0;
            foreach (char ch in plainText)
            {
                total += (int)ch;
            }
            Console.WriteLine($"The checksum of {plainText} is {total}");
        }

        private static void SHA2()
        {
            ASCIIEncoding converter = new ASCIIEncoding();
            string plainText = "Hello World";
            byte[] sourceBytes = converter.GetBytes(plainText);
            HashAlgorithm hasher = SHA256.Create();
            byte[] hash = hasher.ComputeHash(sourceBytes);

            Console.Write("Hashed result: ");
            foreach (byte b in hash)
            {
                Console.Write("{0:X}, ", b);
            }
        }

        private static void DoubleAESEncryption()
        {
            string superSecretStuff = "U.S.A. Nuclear Launch Codes";
            byte[] doubleEncryptedText;
            byte[] key1;
            byte[] key2;
            byte[] initVector1;
            byte[] initVector2;

            using (Aes aes = Aes.Create())
            {
                key1 = aes.Key;
                initVector1 = aes.IV;
                ICryptoTransform encryptor1 = aes.CreateEncryptor();

                using (MemoryStream encryptMemoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream1 =
                        new CryptoStream(encryptMemoryStream, encryptor1, CryptoStreamMode.Write))
                    {
                        using (Aes aes2 = Aes.Create())
                        {
                            key2 = aes2.Key;
                            initVector2 = aes2.IV;
                            ICryptoTransform encryptor2 = aes.CreateEncryptor();

                            using (CryptoStream cryptoStream2 =
                                new CryptoStream(cryptoStream1, encryptor2, CryptoStreamMode.Write))
                            {
                                using (StreamWriter streamWriterEncrypt = new StreamWriter(cryptoStream2))
                                {
                                    streamWriterEncrypt.Write(superSecretStuff);
                                }
                                doubleEncryptedText = encryptMemoryStream.ToArray();
                            }

                        }
                    }
                }
            }
            Console.WriteLine("Double Encrypted Message:");
            foreach (byte b in doubleEncryptedText)
            {
                Console.Write(b);
            }
        }
    }
}
