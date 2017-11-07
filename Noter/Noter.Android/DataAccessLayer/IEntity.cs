using SQLite;

namespace Noter.Droid.DataAccessLayer
{
    public interface IEntity
    {
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
    }
}
