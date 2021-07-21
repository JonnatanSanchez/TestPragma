using MySqlConnector;

namespace GoogleScraping.Infrastructure.Data.Repositories
{
    public interface IAppDatabaseObject
    {
        void Dispose();
        MySqlConnection Connection { get; }
    }
}
