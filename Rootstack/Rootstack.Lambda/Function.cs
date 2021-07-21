using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using GoogleScraping.Application.Services;
using GoogleScraping.Models;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace GoogleScraping.Lambda
{
    public class Function
    {
        private ServiceProvider Provider;
        public Function()
        {
            Provider = new Startup().serviceProvider;
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            ResponseViewModel responseViewModel;
            APIGatewayProxyResponse response = new APIGatewayProxyResponse();

            try
            {
               string command = request.PathParameters["cmd"]?.ToString();

                switch (command)
                {
                    case "Categorias":
                        responseViewModel = GetCategories(request);
                        break;
                    case "PorCategoria":
                        responseViewModel = GetProductsByCategory(request);
                        break;
                    case "PorSubCategoria":
                        responseViewModel = GetProductsBySubCategory(request);
                        break;
                    case "CategoriasDB":
                        responseViewModel = GetCategoriesDB(request);
                        break;
                    case "Products":
                        responseViewModel = GetProducts(request);                        
                        break;
                    case "RefreshCategorias":
                        responseViewModel =  LoadCategoriesToDB(request);
                        break;
                    case "RefreshProductsByCategory":
                        responseViewModel =  LoadProductsByCategory(request);
                        break;
                    case "Scrapear":
                        responseViewModel =  ScrapearAndLoadInDB(request);
                        break;

                    default:
                        throw new Exception("Comando no permitido");
                }
            }
            catch (Exception ex)
            {
                responseViewModel = new ResponseViewModel
                {
                    Message = "Error en el proceso"
                };
                response.StatusCode = 401;
                response.Body = JsonConvert.SerializeObject(responseViewModel);
            }

            return response;
        }

        public ResponseViewModel GetCategories(APIGatewayProxyRequest request)
        {
            ICategoriesApplicationService configurationsApplication = Provider.GetService<ICategoriesApplicationService>();

            return configurationsApplication.GetCategoriesFromMilAnuncios();
        }

        public ResponseViewModel GetCategoriesDB(APIGatewayProxyRequest request)
        {
            ICategoriesApplicationService configurationsApplication = Provider.GetService<ICategoriesApplicationService>();

            return configurationsApplication.GetCategoriesFromDataBase();
        }

        public ResponseViewModel LoadCategoriesToDB(APIGatewayProxyRequest request)
        {
            ICategoriesApplicationService categoriesApplicationService = Provider.GetService<ICategoriesApplicationService>();
            return categoriesApplicationService.LoadCategoriesToDB();
        }

        public ResponseViewModel ScrapearAndLoadInDB(APIGatewayProxyRequest request)
        {
            IFillDataBaseApplicationService fillDataBaseApplicationService = Provider.GetService<IFillDataBaseApplicationService>();
            return fillDataBaseApplicationService.FillDataBase();
        }

        public ResponseViewModel GetProductsByCategory(APIGatewayProxyRequest request)
        {
            string category = request.PathParameters["Categoria"]?.ToString();
            IProductsApplicationService productsApplicationService = Provider.GetService<IProductsApplicationService>();
            return productsApplicationService.GetProductsByCategory(category);
        }

        public ResponseViewModel GetProductsBySubCategory(APIGatewayProxyRequest request)
        {
            string category = request.PathParameters["Categoria"]?.ToString();
            string subCategory = request.PathParameters["SubCategoria"]?.ToString();

            IProductsApplicationService productsApplicationService = Provider.GetService<IProductsApplicationService>();
            return productsApplicationService.GetProductsBySubCategory(category, subCategory);
        }
        public ResponseViewModel GetProducts(APIGatewayProxyRequest request)
        {
            string categoryName = request.PathParameters["Categoria"]?.ToString();
            string productName = request.PathParameters["Producto"]?.ToString();
            string description = request.PathParameters["Descripcion"]?.ToString();

            IProductsApplicationService productsApplicationService = Provider.GetService<IProductsApplicationService>();
            return productsApplicationService.GetProductsByFilter(categoryName, productName, description);
        }

        public ResponseViewModel LoadProductsByCategory(APIGatewayProxyRequest request)
        {
            string categoryName = request.PathParameters["Categoria"]?.ToString();
            IProductsApplicationService productsApplicationService = Provider.GetService<IProductsApplicationService>();
            return productsApplicationService.LoadProductsByCategory(categoryName);
        }


    }
}
