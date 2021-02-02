using CzomPack.Logging;
using System;
using System.ComponentModel;
using System.Security.Cryptography;

namespace CzomPack.Cryptography
{
    public class RSA
    {
        public static String Encrypt(String Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2^16))
                {
                    RSA.ImportParameters(RSAKey);
                    encryptedData = RSA.Encrypt((byte[])new ByteConverter().ConvertFromString(Data), DoOAEPPadding);
                }
                return new ByteConverter().ConvertToString(encryptedData);
            }
            catch (CryptographicException e)
            {
                Logger.Error(e.ToString());
                return null;
            }
        }
        public static String Decrypt(String Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt((byte[])new ByteConverter().ConvertFromString(Data), DoOAEPPadding);
                }
                return new ByteConverter().ConvertToString(decryptedData);
            }
            catch (CryptographicException e)
            {
                Logger.Error(e.ToString());
                return null;
            }
        }
    }
}
