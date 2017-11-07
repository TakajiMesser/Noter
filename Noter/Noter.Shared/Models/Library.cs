using System;
using System.Collections.Generic;
using System.Text;

namespace Noter.Shared.Models
{
    public class Library
    {
        public List<Bookshelf> Bookshelves { get; } = new List<Bookshelf>();

        public Library()
        {

        }
    }
}
