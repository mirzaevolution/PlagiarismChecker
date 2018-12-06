using System.Collections.Generic;

namespace System
{
    public class CustomException : Exception
    {
        public List<string> Errors { get; set; }
    }
}