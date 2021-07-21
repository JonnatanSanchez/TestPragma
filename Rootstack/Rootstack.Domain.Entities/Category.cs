using System.Collections.Generic;

namespace GoogleScraping.Domain.Entities
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public List<SubCategory> children { get; set; }
        public List<string> subCategories { get; set; }
    }
}
