using System;
using System.Collections.Generic;
using System.Text;
using Noter.Shared.DataAccessLayer;
using SQLite;

namespace Noter.Shared.Data
{
    public enum LineStates
    {
        None,
        Checked,
        QuestionMarked,
        Crossed
    }

    public class Line : IEntity
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [Identifier]
        public int LineNr { get; set; }
        public string Text { get; set; }
        public LineStates State { get; set; }

        [ForeignKey(typeof(Page))]
        public int PageID { get; set; }

        [Ignore]
        public Page Page => DBTable.Get<Page>(PageID);
    }
}
