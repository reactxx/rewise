using System;
using System.Collections.Generic;
using System.Text;

namespace bank.model
{
    public class Book
    {
        public int BookId { get; set; }
        public string Path { get; set; }

        public ICollection<Facts> Factss { get; set; }
    }
    public class Facts
    {
        public int FactsId { get; set; }
        public Book Book { get; set; }
    }
}
