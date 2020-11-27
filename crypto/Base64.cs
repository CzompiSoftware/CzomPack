using System;
using System.Text;

namespace hu.czompisoftware.libraries.crypto
{
    /**
     * <summary>
     * Base64 helper class.<br/>
     * Copyright Czompi Software 2020<br/>
     * File version <b>1.1</b> | Author <b>Czompi</b>
     * </summary>
     */
    public class Base64
    {
        public static String Encode(String str)
        {
            //return Reverse(Convert.ToBase64String(Encoding.UTF8.GetBytes(str)));
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        public static String Encode2(Byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static String Decode(String str)
        {
            //return Encoding.UTF8.GetString(Convert.FromBase64String(Reverse(str)));
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }
        public static String Decode2(String str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }
    }
}
