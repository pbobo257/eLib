using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities.Exceptions
{
    public class PasswordsNotMatchException : Exception
    {
        public PasswordsNotMatchException() : base("Passwords don’t match")
        {
        }
    }
}
