﻿using System;
using System.Runtime.Serialization;

namespace CzomPack.Translation
{
    [Serializable]
    internal class MissingLanguageException : Exception
    {
        public MissingLanguageException()
        {
        }

        public MissingLanguageException(string message) : base(message)
        {
        }

        public MissingLanguageException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingLanguageException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}