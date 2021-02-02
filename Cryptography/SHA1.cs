using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CzomPack.Cryptography
{
    public class SHA1
    {
        public static string Encode(string text)
        {
            return BitConverter.ToString(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
        }
    }
}
