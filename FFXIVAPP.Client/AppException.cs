// FFXIVAPP.Client
// AppException.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Runtime.Serialization;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    [DoNotObfuscate]
    [Serializable]
    public class AppException : Exception
    {
        public AppException()
        {
        }

        public AppException(string message) : base(message)
        {
        }

        public AppException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AppException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
