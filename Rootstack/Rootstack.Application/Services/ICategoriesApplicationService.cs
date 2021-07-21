using Newtonsoft.Json.Linq;
using GoogleScraping.Models;
using System.Threading.Tasks;

namespace GoogleScraping.Application.Services
{
    public interface ICategoriesApplicationService
    {
        ResponseViewModel GetCategoriesFromMilAnuncios();
        ResponseViewModel GetCategoriesFromDataBase();
        ResponseViewModel LoadCategoriesToDB();
    }
}
