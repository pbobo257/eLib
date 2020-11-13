using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Entities
{
    public class BookDetails : IEntity<int>
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public byte[] Book { get; set; }

        public int HeaderId { get; set; }
        public BookHeader Header { get; set; }
    }
}
