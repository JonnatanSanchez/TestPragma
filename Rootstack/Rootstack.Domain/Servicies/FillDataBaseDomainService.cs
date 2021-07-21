using GoogleScraping.Domain.Entities;
using GoogleScraping.Domain.ServiceAgent;
using GoogleScraping.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleScraping.Domain.Servicies
{
    public class FillDataBaseDomainService: IFillDataBaseDomainService
    {
        private readonly ICategoriesDomainService CategoriesDomainService;
        private readonly IProductsDomainService ProductsDomainService;

        public FillDataBaseDomainService(
    ICategoriesDomainService categoriesDomainService,
    IProductsDomainService productsDomainService
    )
        {
            CategoriesDomainService = categoriesDomainService;
            ProductsDomainService = productsDomainService;
        }

        public ResponseViewModel FillDataBase()
        {
            try
            {
                List<Category> categories = CategoriesDomainService.ScrapearAndLoadInDB();

                ResponseViewModel response = new ResponseViewModel();
                foreach (var category in categories)
                {
                    response = ProductsDomainService.LoadProductsByCategory(category.name);
                }
                return response;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
