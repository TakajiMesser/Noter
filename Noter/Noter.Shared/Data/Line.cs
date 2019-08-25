using System;
using System.Collections.Generic;
using System.Text;
using Noter.Shared.DataAccessLayer;
using SQLite;

namespace Noter.Shared.Data
{
    public enum ContentTypes
    {
        Text,
        Numbered,
        Checkbox,
        Image
    }

    public class Line : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Identifier]
        public int LineNr { get; set; }
        public ContentTypes ContentType { get; set; }
        public string Content { get; set; }

        [ForeignKey(typeof(Page))]
        public int PageID { get; set; }

        [ForeignKey(typeof(Line))]
        public int ParentLineID { get; set; }

        [Ignore]
        public Page Page => DBTable.Get<Page>(PageID);

        [Ignore]
        public Line ParentLine => DBTable.Get<Line>(ParentLineID);

        [Ignore]
        public TableQuery<Line> ChildLines => DBTable.GetAll<Line>(l => l.ParentLineID == ID);
    }
}
