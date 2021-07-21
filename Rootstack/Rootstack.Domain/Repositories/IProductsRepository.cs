using GoogleScraping.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleScraping.Domain.Repositories
{
    public interface IProductsRepository : IRepository<Product>
    {
        List<Product> GetByFilter(ProductFilters productFilters);
        bool InsertProducts(List<Product> products);
        bool RemoveAllProductsByCategory(int categoryId);
    }
}
