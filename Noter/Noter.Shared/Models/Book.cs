using System;
using System.Collections.Generic;
using System.Text;

namespace Noter.Shared.Models
{
    public class Book
    {
        public string Title { get; set; }
        public List<Page> Pages { get; } = new List<Page>();

        public Book()
        {

        }
    }
}
