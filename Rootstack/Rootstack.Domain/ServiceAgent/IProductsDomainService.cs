using GoogleScraping.Models;
using System.Threading.Tasks;

namespace GoogleScraping.Domain.ServiceAgent
{
    public interface IProductsDomainService
    {
        ResponseViewModel GetProductsByCategory(string categoryName);
        ResponseViewModel GetProductsBySubCategory(string categoryName, string subCategory);
        ResponseViewModel GetProductsByFilter(string categoryName, string productName, string description);
        ResponseViewModel LoadProductsByCategory(string categoryName);
    }
}
