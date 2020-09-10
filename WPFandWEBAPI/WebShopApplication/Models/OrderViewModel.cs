using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Persistence;

namespace WebShopApplication.Models
{
    public class OrderViewModel
    {
        string OrderAsc { get; set; }
        public List<Product> ProductsVM { get; set; }

        public List<SelectListItem> OrdersList { get; set; }

        public List<SelectListItem> OrderByPriceList { get; set; }
        public string OrderByPriceString { get; set; }

        public string OrderString { get; set; }

        public int ItemCounter { get; set; }
    }
}
