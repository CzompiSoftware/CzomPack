using System;
using System.Runtime.Serialization;

namespace hu.czompisoftware.libraries.license.exception
{
    /**
     * <summary>
     * This class is a simple Exception type class for detecting unlicensed software.<br/>
     * Copyright Czompi Software 2020<br/>
     * File version <b>1.0.1</b> | Author <b>Czompi</b>
     * </summary>
     */
    [Serializable]
    public class SoftwareFreeUse : Exception
    {
        public SoftwareFreeUse()
        {
        }

        public SoftwareFreeUse(string message) : base(message)
        {
        }

        public SoftwareFreeUse(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SoftwareFreeUse(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}