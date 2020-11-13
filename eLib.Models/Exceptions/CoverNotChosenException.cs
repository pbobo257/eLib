using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities.Exceptions
{
    public class CoverNotChosenException : Exception
    {
        public CoverNotChosenException() : base("Cover not chosen")
        {
        }
    }
}
