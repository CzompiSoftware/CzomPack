using System;
using System.Text;

namespace CzomPack.Cryptography
{
    /**
     * <summary>
     * Base64 helper class.<br/>
     * Copyright Czompi Software 2020<br/>
     * File version <b>1.2.1</b> | Author <b>Czompi</b>
     * </summary>
     */
    public class Base64
    {
        public static String Encode(String str) => Convert.ToBase64String(Encoding.UTF8.GetBytes(str));

        public static String Decode(String str) => Encoding.UTF8.GetString(Convert.FromBase64String(str));
    }
}
