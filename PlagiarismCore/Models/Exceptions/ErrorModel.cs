using System.Collections.Generic;

namespace System
{
    public class ErrorModel
    {
        public List<string> Errors { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}