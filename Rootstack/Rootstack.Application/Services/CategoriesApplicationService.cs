using AutoMapper;
using GoogleScraping.Domain.ServiceAgent;
using GoogleScraping.Models;

namespace GoogleScraping.Application.Services
{
    public class CategoriesApplicationService : ICategoriesApplicationService
    {
        private readonly ICategoriesDomainService categoriesDomainService;
        private readonly IProductsDomainService productsDomainService;
        private readonly IMapper mapper;

        public CategoriesApplicationService(ICategoriesDomainService categoriesDomainService, IMapper mapper,
            IProductsDomainService productsDomainService)
        {
            this.categoriesDomainService = categoriesDomainService;
            this.productsDomainService = productsDomainService;
            this.mapper = mapper;
        }

        public ResponseViewModel GetCategoriesFromMilAnuncios()
        {
            return categoriesDomainService.GetCategoriesFromMilAnuncios();
        }

        public ResponseViewModel GetCategoriesFromDataBase()
        {
            return categoriesDomainService.GetCategoriesFromDataBase();
        }

        public ResponseViewModel LoadCategoriesToDB()
        {
            return  categoriesDomainService.LoadCategoriesToDB();
        }
    }
}
