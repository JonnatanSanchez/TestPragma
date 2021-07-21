using MySqlConnector;
using System;

namespace GoogleScraping.Infrastructure.Data.Repositories
{
    public class AppDatabaseObject : IDisposable, IAppDatabaseObject
    {
        public MySqlConnection Connection { get; }

        public AppDatabaseObject()
        {
            Connection = new MySqlConnection("Server=db4free.net;Uid=karenjulieth;Pwd=heraldry0;Database=milanuncios");
        }

        public void Dispose() => Connection.Dispose();
    }
}
