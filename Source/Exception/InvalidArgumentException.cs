using System;

namespace SharpEngine
{
    internal class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string message) : base(message)
        {

        }
    }
}
