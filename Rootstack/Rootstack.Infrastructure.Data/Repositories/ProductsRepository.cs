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
    public class ProductsRepository : IProductsRepository
    {
        private readonly IAppDatabaseObject appDatabaseObject;

        public ProductsRepository(IAppDatabaseObject appDatabase)
        {
            appDatabaseObject = appDatabase;

            if (!appDatabaseObject.Connection.State.HasFlag(ConnectionState.Open))
            {
                appDatabaseObject.Connection.Open();
            }
        }
        public bool Add(Product obj)
        {
            throw new NotImplementedException();
        }

        public bool AddMasive(List<Product> lstObj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Product> GetByFilter(ProductFilters productFilters)
        {
            MySqlCommand cmd = appDatabaseObject.Connection.CreateCommand();
            //
            ExecuteQueryFilters(ref cmd, productFilters);
            List<Product> result =  ReadAll(cmd.ExecuteReader());

            if (productFilters.categoryId != 0)
            {
                if (!string.IsNullOrEmpty(productFilters.name))
                {
                    result.Where(x => x.name.Contains(productFilters.name));
                }
                if (!string.IsNullOrEmpty(productFilters.description))
                {
                    result.Where(x => x.description.Contains(productFilters.description));
                }
            }
            else if (!string.IsNullOrEmpty(productFilters.name))
            {
                if (!string.IsNullOrEmpty(productFilters.description))
                {
                    result.Where(x => x.description.Contains(productFilters.description));
                }
            }

            return result.Count > 0 ? result : null;
        }

        private void ExecuteQueryFilters(ref MySqlCommand cmd, ProductFilters productFilters)
        {
            if (productFilters.categoryId != 0)
            {
                cmd.CommandText = @"SELECT * FROM `product` where `categoryId` = @category";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@category",
                    DbType = DbType.Int32,
                    Value = productFilters.categoryId
                });
            }
            else if (!string.IsNullOrEmpty(productFilters.name))
            {
                cmd.CommandText = @"SELECT * FROM `product` where `name` LIKE %@name%";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@name",
                    DbType = DbType.Int32,
                    Value = productFilters.name
                });
            }
            else if (!string.IsNullOrEmpty(productFilters.description))
            {
                cmd.CommandText = @"SELECT * FROM `Product` where `description` LIKE %@description%";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@description",
                    DbType = DbType.Int32,
                    Value = productFilters.description
                });
            }
            else
            {
                cmd.CommandText = @"SELECT * FROM `product`";
            }
        }
        private List<Product> ReadAll(DbDataReader reader)
        {
            var posts = new List<Product>();
            using (reader)
            {
                while ( reader.Read())
                {
                    var post = new Product()
                    {
                        productId = reader.GetInt32(0),
                        name = reader.GetString(1),
                        type = reader.GetString(2),
                        localization = reader.GetString(3),
                        description = reader.GetString(4),
                        readMore = reader.GetString(5),
                        urlImages = reader.GetString(6).Split(',').ToList(),
                        seller = reader.GetString(7),
                        price = reader.GetString(8),
                        sellerType = reader.GetString(9),
                        reference = reader.GetString(10),
                        attributes = reader.GetString(11).Split(',').ToList(),
                        categoryId = reader.GetInt32(12)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        public bool RemoveAllProductsByCategory(int categoryId)
        {
            try
            {
                MySqlTransaction txn = appDatabaseObject.Connection.BeginTransaction();
                MySqlCommand cmd = appDatabaseObject.Connection.CreateCommand();
                cmd.CommandText = @"DELETE FROM `product` where categoryId = @category";
                cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@category",
                    DbType = DbType.Int32,
                    Value = categoryId
                });
                cmd.ExecuteNonQuery();
                txn.Commit();
                return true;
            }
            catch
            {
                return false;
            }            
        }

        public bool InsertProducts(List<Product> products)
        {
            try
            {
                StringBuilder sCommand = new StringBuilder("INSERT INTO product (name, type, localization, description, readMore, urlImages, seller, price, sellerType, reference, attributes, categoryId) VALUES ");
                using (appDatabaseObject.Connection)
                {
                    List<string> Rows = new List<string>();
                    foreach (var product in products)
                    {
                        Rows.Add(string.Format("('{0}','{1}','{2}','{3}')", product.name,
                            product.type, product.localization, product.description,
                            product.readMore, product.urlImages.ToString(), product.seller,
                            product.price, product.sellerType, product.reference,
                            product.attributes.ToString(), product.categoryId
                            ));
                    }
                    sCommand.Append(string.Join(",", Rows));
                    sCommand.Append(";");

                    MySqlCommand cmd = appDatabaseObject.Connection.CreateCommand();

                    cmd.CommandText = sCommand.ToString();
                    var result = ReadAll(cmd.ExecuteReader());
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public Product GetById(long id, int range)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Product obj)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public bool Update(Product obj)
        {
            throw new NotImplementedException();
        }
    }
}
