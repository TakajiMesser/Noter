using System;
using System.Collections.Generic;
using System.Text;
using Noter.Shared.DataAccessLayer;
using SQLite;

namespace Noter.Shared.Data
{
    [Table("Libraries")]
    public class Library : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Identifier]
        public string Name { get; set; }

        [Ignore]
        public TableQuery<Shelf> Shelves => DBTable.GetAll<Shelf>(b => b.LibraryID == ID);
    }
}
