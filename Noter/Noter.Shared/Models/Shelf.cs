using System;
using System.Collections.Generic;
using System.Text;

namespace Noter.Shared.Models
{
    public class Shelf
    {
        public List<Book> Books { get; } = new List<Book>();

        public Shelf()
        {

        }

        public void DoSomeShit()
        {
            int a = 3;
            a = 4;
            a += 5;

            if (a == 9)
            {
                a = 3;
            }
        }
    }
}
