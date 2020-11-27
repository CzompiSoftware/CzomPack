using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace hu.czompisoftware.libraries.crypto
{
    public class SHA1
    {
        private string Text { get; }

        public SHA1(String text)
        {
            this.Text = text;
        }

        public override String ToString()
        {
            var sha1 = new SHA1Managed();
            return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(Text)));
        }
    }
}
