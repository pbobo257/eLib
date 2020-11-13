using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities.Exceptions
{
    public class BookNotChosenException : Exception
    {
        public BookNotChosenException() : base("Book not chosen")
        {
        }
    }
}
