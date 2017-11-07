using SQLite;

namespace Noter.Shared.DataAccessLayer
{
    public interface IEntity
    {
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
    }
}
