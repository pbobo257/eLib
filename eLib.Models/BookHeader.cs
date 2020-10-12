using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities
{
    public class BookHeader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Cover { get; set; }

        public BookDetails Details { get; set; }
    }
}
