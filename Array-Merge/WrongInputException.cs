using System;

namespace Array_Merge
{
    public class WrongInputException : Exception
    {
        public WrongInputException()
        {
        }

        public WrongInputException(string message)
            : base(message)
        {
        }

        public WrongInputException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}