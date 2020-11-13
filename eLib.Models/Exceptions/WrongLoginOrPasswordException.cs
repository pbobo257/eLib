using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities.Exceptions
{
    public class WrongLoginOrPasswordException : Exception
    {
        public WrongLoginOrPasswordException() : base("Wrong login or password")
        {
        }
    }
}
