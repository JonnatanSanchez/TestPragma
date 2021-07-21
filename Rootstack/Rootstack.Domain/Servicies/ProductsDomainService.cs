using HtmlAgilityPack;
using GoogleScraping.Domain.Entities;
using GoogleScraping.Domain.Repositories;
using GoogleScraping.Domain.ServiceAgent;
using GoogleScraping.Models;
using ScrapySharp.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoogleScraping.Domain.Servicies
{
    public class ProductsDomainService : IProductsDomainService
    {
        private readonly ICategoriesDomainService CategotyDomainService;
        private readonly IProductsRepository ProductsRepository;

        public ProductsDomainService(
    ICategoriesDomainService categoryDomainService,
    IProductsRepository productsDomainService
    )
        {
            CategotyDomainService = categoryDomainService;
            ProductsRepository = productsDomainService;
        }

        public ResponseViewModel GetProductsByCategory(string categoryName)
        {
            ResponseViewModel response = new ResponseViewModel();
            response.Data = GetProductsByCategoryList(categoryName);
            response.Message = "Ok";
            return response;
        }

        public ResponseViewModel GetProductsBySubCategory(string categoryName, string subCategory)
        {
            ResponseViewModel response = new ResponseViewModel();
            response.Data = GetProductsBySubCategoryList(categoryName, subCategory);
            response.Message = "Ok";
            return response;
        }

        public ResponseViewModel GetProductsByFilter(string categoryName, string productName, string description)
        {
            ResponseViewModel response = new ResponseViewModel();
            response.Data = GetProductsByFiltersList(categoryName, productName, description);
            response.Message = "Ok";
            return response;
        }

        public ResponseViewModel LoadProductsByCategory(string categoryName)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                Category category = CategotyDomainService.GetCategorieByName(categoryName);
                ProductsRepository.RemoveAllProductsByCategory(category.id);
                List<Product> products = GetProductsByCategoryList(categoryName);
                _ = ProductsRepository.InsertProducts(products);
                response.Message = "Ok";
                return response;
            }
            catch (System.Exception ex)
            {
                response.Message = ex.Message;
                response.Data = ex;
                return response;
                throw;
            }

        }

        private List<Product> GetProductsByFiltersList(string categoryName, string productName, string description)
        {
            Category category = CategotyDomainService.GetCategorieByName(categoryName);

            ProductFilters productFilters = new ProductFilters
            {
                name = productName,
                description = description,
                categoryId = category.id
            };
            return ProductsRepository.GetByFilter(productFilters);
        }

        private List<Product> GetProductsByCategoryList(string categoryName)
        {
            List<Category> categories = CategotyDomainService.GetCategoriesFromMilAnunciosList();

            Category category1 = categories.Find(x => x.name == categoryName);
            return GetProductsByPath(category1.path);
        }


        private List<Product> GetProductsBySubCategoryList(string categoryName, string subCategory)
        {
            List<Category> categories = CategotyDomainService.GetCategoriesFromMilAnunciosList();

            Category category1 = categories.Find(x => x.name == categoryName);
            SubCategory sub = category1.children.Find(x => x.name == subCategory);
            return GetProductsByPath(sub.path);
        }


        private List<Product> GetProductsByPath(string path)
        {
            List<Product> products = new List<Product>();
            HtmlWeb oWeb = new HtmlWeb();
            HtmlDocument doc = oWeb.Load("https://www.milanuncios.com" + path);

            var productsList = doc.DocumentNode.CssSelect("div.aditem");

            foreach (var item in productsList)
            {
                Product product = ConvertHtmlToProduct(item);
                products.Add(product);
            }

            return products;
        }

        private Product ConvertHtmlToProduct(HtmlNode htmlNode)
        {
            Product product = new Product();
            try
            {
                product.type = htmlNode.CssSelect("div.x3").First().InnerText;
                product.reference = Regex.Unescape(htmlNode.CssSelect("div.x5").First().InnerText).Replace("\n  ", "").Trim(' ');
                product.name = htmlNode.CssSelect("a.aditem-detail-title").First().InnerText;
                product.seller = htmlNode.CssSelect("div.list-location-link").First().InnerText;
                product.localization = htmlNode.CssSelect("div.list-location-region").First().InnerText;

                product.description = htmlNode.CssSelect("div.tx").First().InnerText;
                IEnumerable<HtmlNode> readMoreVariable = htmlNode.CssSelect("span.readMoreHidden");
                if (readMoreVariable.Count() > 0)
                {
                    product.readMore = readMoreVariable.First().InnerText;
                }

                IEnumerable<HtmlNode> priceVariable = htmlNode.CssSelect("div.aditem-price");
                if (priceVariable.Count() > 0)
                {
                    product.price = priceVariable.First().InnerText.Replace("&euro;", " €");
                }
                else
                {
                    product.price = "0";
                }

                product.sellerType = htmlNode.CssSelect("div.pillDiv").First().InnerText;

                IEnumerable<HtmlNode> imagesNode = htmlNode.CssSelect("div.aditem-image");
                if (imagesNode.Count() > 0)
                {
                    IEnumerable<string> imagesList = htmlNode.CssSelect("div.aditem-image").First().InnerHtml.Split().Where(x => x.Contains(".jpg?"));
                    product.urlImages = convertHtmlToImagesArray(imagesList);
                }
                
                IEnumerable<HtmlNode> attributesList = htmlNode.CssSelect("div.inmo-attributes").First().SelectNodes("div");
                product.attributes = convertHtmlToAttributesArray(attributesList);
            }
            catch (System.Exception ex)
            {

                throw;
            }            
            return product;
        }

        private List<string> convertHtmlToImagesArray(IEnumerable<string> htmlInformation)
        {
            List<string> imagesList = new List<string>();
            try
            {
                int position = 0;
                string imageUrl = "";

                foreach (var item in htmlInformation)
                {
                    position = item.IndexOf(".jpg?");
                    imageUrl = item.Substring(0, position + 4).Replace("\"", "").TrimStart('\'');
                    imagesList.Add(imageUrl);
                }

                if (imagesList.Count > 1)
                {
                    imagesList.RemoveAt(imagesList.Count - 1);
                }
            }
            catch (System.Exception ex)
            {

                throw;
            }         
            return imagesList;
        }

        private List<string> convertHtmlToAttributesArray(IEnumerable<HtmlNode> htmlInformation)
        {
            List<string> attributesList = new List<string>();

            if (htmlInformation == null)
            {
                return attributesList;
            }

            foreach (var item in htmlInformation)
            {
                attributesList.Add(item.InnerText);
            }
            return attributesList;
        }

    }
}
