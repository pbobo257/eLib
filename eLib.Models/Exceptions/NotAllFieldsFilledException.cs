using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities.Exceptions
{
    public class NotAllFieldsFilledException : Exception
    {
        public NotAllFieldsFilledException() : base("Not all fields are filled")
        {
        }
    }
}
