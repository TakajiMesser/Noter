using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Noter.Droid.Models
{
    public class Bookshelf
    {
        public List<Book> Books { get; } = new List<Book>();

        public Bookshelf()
        {

        }
    }
}