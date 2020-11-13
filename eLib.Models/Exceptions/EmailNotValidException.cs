using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities.Exceptions
{
    public class EmailNotValidException : Exception
    {
        public EmailNotValidException() :  base("This is not valid e-mail")
        {
        }
    }
}
