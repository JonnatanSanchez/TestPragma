using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleScraping.Domain.Entities
{
    public class Product
    {
        public int productId { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string localization { get; set; }
        public string description { get; set; }
        public string readMore { get; set; }
        public List<string> urlImages { get; set; }
        public string seller { get; set; }
        public string price { get; set; }
        public string sellerType { get; set; }
        public string reference { get; set; }
        public List<string> attributes { get; set; }
        public int categoryId { get; set; }
    }
}
