using GoogleScraping.Domain.Entities;
using GoogleScraping.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleScraping.Domain.ServiceAgent
{
    public interface ICategoriesDomainService
    {
        ResponseViewModel GetCategoriesFromMilAnuncios();
        ResponseViewModel GetCategoriesFromDataBase();
        List<Category> GetCategoriesFromMilAnunciosList();
        Category GetCategorieByName(string name);
        ResponseViewModel LoadCategoriesToDB();
        List<Category> ScrapearAndLoadInDB();
    }
}
