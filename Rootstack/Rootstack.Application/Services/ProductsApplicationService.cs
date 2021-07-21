using AutoMapper;
using GoogleScraping.Domain.ServiceAgent;
using GoogleScraping.Models;

namespace GoogleScraping.Application.Services
{
    public class ProductsApplicationService : IProductsApplicationService
    {
        private readonly IProductsDomainService productsDomainService;
        private readonly IMapper mapper;

        public ProductsApplicationService(IProductsDomainService productsDomainService, IMapper mapper)
        {
            this.productsDomainService = productsDomainService;
            this.mapper = mapper;
        }

        public ResponseViewModel GetProductsByCategory(string categoryName)
        {
            return productsDomainService.GetProductsByCategory(categoryName);
        }

        public ResponseViewModel GetProductsBySubCategory(string categoryName, string subCategory)
        {
            return productsDomainService.GetProductsBySubCategory(categoryName, subCategory);
        }

        public ResponseViewModel GetProductsByFilter(string categoryName, string productName, string description)
        {
            return productsDomainService.GetProductsByFilter(categoryName, productName, description);
        }

        public ResponseViewModel LoadProductsByCategory(string categoryName)
        {
            return productsDomainService.LoadProductsByCategory(categoryName);
        }
    }
}
