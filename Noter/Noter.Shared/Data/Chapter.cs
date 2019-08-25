using System;
using System.Collections.Generic;
using System.Text;
using Noter.Shared.DataAccessLayer;
using SQLite;

namespace Noter.Shared.Data
{
    public class Chapter : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Identifier]
        public int PageNr { get; set; }

        [ForeignKey(typeof(Book))]
        public int BookID { get; set; }

        [Ignore]
        public Book Book => DBTable.Get<Book>(BookID);
    }
}
