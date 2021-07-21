using MySqlConnector;
using GoogleScraping.Domain.Entities;
using GoogleScraping.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleScraping.Infrastructure.Data.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IAppDatabaseObject appDatabaseObject;

        public CategoriesRepository(IAppDatabaseObject appDatabase)
        {
            appDatabaseObject = appDatabase;

            if (!appDatabaseObject.Connection.State.HasFlag(ConnectionState.Open))
            {
                appDatabaseObject.Connection.Open();
            }
        }

        public bool Add(Category obj)
        {
            throw new System.NotImplementedException();
        }

        public bool AddMasive(List<Category> lstObj)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public List<Category> GetAll()
        {
            var cmd = appDatabaseObject.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `category`";
            var result = ReadAll(cmd.ExecuteReader());
            return result.Count > 0 ? result : null;
        }

        private List<Category> ReadAll(DbDataReader reader)
        {
            var posts = new List<Category>();
            using (reader)
            {
                while (reader.Read())
                {
                    var post = new Category()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        subCategories = reader.GetString(3).Split(',').ToList(),
                        path = reader.GetString(2)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        public Category GetById(long id, int range)
        {
            throw new NotImplementedException();
        }

        public Category GetByName(string name)
        {
            MySqlCommand cmd = appDatabaseObject.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM `category` where `name` = @name";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name,
            });

            var result = ReadAll(cmd.ExecuteReader());
            return result.Count > 0 ? result[0] : null;
        }

        public bool Remove(Category obj)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveAll()
        {
            try
            {         
                MySqlCommand cmd = appDatabaseObject.Connection.CreateCommand();
                cmd.CommandText = "DELETE FROM `category`";
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertCategories(List<Category> categories)
        {
            try
            {
                StringBuilder sCommand = new StringBuilder("INSERT INTO category (id, name, subCategories, path) VALUES ");
                using (appDatabaseObject.Connection)
                {
                    List<string> Rows = new List<string>();
                    foreach (var category in categories)
                    {
                        category.subCategories = new List<string>();
                        string subCategories = "";
                        foreach (var item in category.children)
                        {
                            subCategories += "," + item.name;
                        }
                        subCategories.TrimStart(',');
                        Rows.Add(string.Format("('{0}','{1}','{2}','{3}')", category.id, category.name, subCategories, category.path));
                    }
                    sCommand.Append(string.Join(",", Rows));
                    sCommand.Append(";");

                    MySqlCommand cmd = appDatabaseObject.Connection.CreateCommand();

                    cmd.CommandText = sCommand.ToString();
                    var result = ReadAll(cmd.ExecuteReader());
                }
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
                throw;
            }
        }

        public int SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Category obj)
        {
            throw new System.NotImplementedException();
        }

        Task<List<Category>> IRepository<Category>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
