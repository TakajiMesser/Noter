using System;
using System.Collections.Generic;
using System.Text;
using Noter.Shared.DataAccessLayer;
using SQLite;

namespace Noter.Shared.Data
{
    [Table("Books")]
    public class Book : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Identifier]
        public string Title { get; set; }

        [ForeignKey(typeof(Shelf))]
        public int ShelfID { get; set; }

        [ForeignKey(typeof(Tag))]
        public List<int> TagIDs { get; set; }

        [Ignore]
        public Shelf Shelf => DBTable.Get<Shelf>(ShelfID);

        [Ignore]
        public TableQuery<Page> Pages => DBTable.GetAll<Page>(p => p.BookID == ID);
    }
}
