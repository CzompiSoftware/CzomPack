using System;
using System.Runtime.Serialization;

namespace hu.czompisoftware.libraries.license.exception
{
    [Serializable]
    public class SoftwareLicenseException : Exception
    {
        public SoftwareLicenseException()
        {
        }

        public SoftwareLicenseException(string message) : base(message)
        {
        }

        public SoftwareLicenseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SoftwareLicenseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}