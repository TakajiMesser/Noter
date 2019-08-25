using System;
using System.Collections.Generic;
using System.Text;
using Noter.Shared.DataAccessLayer;
using SQLite;

namespace Noter.Shared.Data
{
    // [Column(name)], [Table(name)], [MaxLength(value)], [Ignore], [Unique] 
    [Table("Shelves")]
    public class Shelf : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Identifier]
        public string Name { get; set; }

        [ForeignKey(typeof(Library))]
        public int LibraryID { get; set; }

        [ForeignKey(typeof(Tag))]
        public List<int> TagIDs { get; set; }

        [Ignore]
        public Library Library => DBTable.Get<Library>(LibraryID);

        [Ignore]
        public TableQuery<Book> Books => DBTable.GetAll<Book>(b => b.ShelfID == ID);
    }
}
