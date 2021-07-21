using GoogleScraping.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleScraping.Domain.Repositories
{
    public interface ICategoriesRepository : IRepository<Category>
    {
        Category GetByName(string name);
        bool RemoveAll();
        bool InsertCategories(List<Category> categories);
        new List<Category> GetAll();
    }
}
