using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class CategoriesDomainService : ICategoriesDomainService
    {
        private readonly ICategoriesRepository CategoriesRepository;

        public CategoriesDomainService(
            ICategoriesRepository categoriesRepository
            )
        {
            CategoriesRepository = categoriesRepository;
        }

        public ResponseViewModel GetCategoriesFromMilAnuncios()
        {
            ResponseViewModel response = new ResponseViewModel();
            response.Data = GetCategoriesFromMilAnunciosList();
            response.Message = "Ok";
            return response;
        }

        public List<Category> GetCategoriesFromMilAnunciosList()
        {
            HtmlWeb oWeb = new HtmlWeb();
            HtmlDocument doc = oWeb.Load("https://www.milanuncios.com/");
            JObject scriptsList = new JObject();
            List<Category> categories = new List<Category>();

            string categoriesList = doc.DocumentNode.CssSelect("script").Where
                (x => x.InnerHtml.Contains("categories")).First().InnerText;

            transformInnerText(ref categoriesList);
            scriptsList = (JObject)JsonConvert.DeserializeObject(categoriesList);
            removePropertiesJson(ref scriptsList);
            scriptsList = (JObject)JsonConvert.DeserializeObject(categoriesList);

            categories = JsonConvert.DeserializeObject<List<Category>>(scriptsList.First.First.ToString());
            return categories;
        }

        public ResponseViewModel GetCategoriesFromDataBase()
        {
            ResponseViewModel response = new ResponseViewModel();
            response.Data = GetCategoriesFromDataBaseList();
            response.Message = "Ok";
            return response;
        }

        public List<Category> GetCategoriesFromDataBaseList()
        {
            return CategoriesRepository.GetAll();
        }

        public List<Category> ScrapearAndLoadInDB()
        {
            bool statusRemove = CategoriesRepository.RemoveAll();
            List<Category> categories = GetCategoriesFromMilAnunciosList();
            bool statusInsert = CategoriesRepository.InsertCategories(categories);
            return categories;
        }

        private void transformInnerText(ref string categoriesList)
        {
            categoriesList = categoriesList.Replace("window.__INITIAL_PROPS__ = JSON.parse(", "");
            categoriesList = categoriesList.Replace(");", "");
            categoriesList = categoriesList.TrimStart('"');
            categoriesList = categoriesList.TrimEnd('"');
            categoriesList = Regex.Unescape(categoriesList);
        }

        private void removePropertiesJson(ref JObject categoriesList)
        {
            List<string> propietiesToDelete = new List<string>();
            foreach (var item in categoriesList)
            {
                if (item.Key != "categories")
                {
                    propietiesToDelete.Add(item.Key);
                }

            }

            // Se realiza el recorrido en un forEach diferente para evitar mutabilidad
            // aprovechando que la cantidad de propiedades siempre serán pocas.
            foreach (var item in propietiesToDelete)
            {
                categoriesList.Remove(item);
            }

        }

        public Category GetCategorieByName(string name)
        {
            return CategoriesRepository.GetByName(name);
        }

        public ResponseViewModel LoadCategoriesToDB()
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                
                CategoriesRepository.RemoveAll();
                List<Category> categories = GetCategoriesFromMilAnunciosList();
                CategoriesRepository.InsertCategories(categories);
                response.Message = "OK";
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

    }
}
