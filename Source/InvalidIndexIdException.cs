using System;
using System.Runtime.Serialization;

namespace SharpEngine
{
    internal class InvalidIndexIdException : Exception
    {
        public InvalidIndexIdException(string message) : base(message)
        {

        }
    }
}