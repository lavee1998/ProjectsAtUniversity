using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;

namespace WebShopApplication.Models
{
    public class CartViewModel
    {
        
        public Dictionary<Product,int> _productcart { get; set; }

        public int ItemCounter { get; set; }
    }
}
