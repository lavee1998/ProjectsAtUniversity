using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApplication.Data;
using WebShopApplication.Models;

namespace WebShopApplication.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MvcWebShopContext _context;

        public ShoppingCartController(MvcWebShopContext context)
        {
            _context = context;
        }

        // GET: Categories
        public IActionResult Index()
        {
            Dictionary<int, int> _cart = new Dictionary<int, int>();
            _cart = HttpContext.Session.Get<Dictionary<int, int>>("ShoppingCart");
            
            var _shoppingcartVM = new CartViewModel();
            _shoppingcartVM._productcart = new Dictionary<Product, int>();
            List<int> keyList;
            int _currentKey;
            if (_cart != null)
            {
                keyList = new List<int>(_cart.Keys);
                for (int i = 0; i < _cart.Count(); i++)
                {
                    _currentKey = keyList[i];
                    _shoppingcartVM._productcart.Add(_context.Products.Find(_currentKey), _cart[_currentKey]);
                }

                ViewBag.ShoppingCartCounter = _shoppingcartVM._productcart.Sum(x => x.Value);
                List<Product> _productList = new List<Product>(_shoppingcartVM._productcart.Keys);
                for (int i = 0; i < _productList.Count; i++)
                {
                    ViewBag.ShoppingCartNetPrice += _productList[i].NetPrice * _shoppingcartVM._productcart[_productList[i]];
                }
            }
            else
            {
                HttpContext.Session.Set("ShoppingCart", new Dictionary<int, int>());
            }

            return View(_shoppingcartVM);
        }

        public IActionResult EditShopCart(int? id, int ItemCounter = 1)
        {
            Dictionary<int, int> _cart = new Dictionary<int, int>();
            _cart = HttpContext.Session.Get<Dictionary<int, int>>("ShoppingCart");
          
            List<int> keyList;
            int difference; 
            if (_cart != null && ItemCounter >= 0)
            {
                keyList = new List<int>(_cart.Keys);
                difference =   ItemCounter - _cart[(int)id];
                if(difference > 0 ) 
                {
                    if(difference <= _context.Products.Find(id).Stock)
                    {
                        _context.Products.Find(id).Stock -= difference;
                        _context.SaveChanges();
                        _cart[(int)id] = ItemCounter;
                    }      
                }
                else
                {
                    _context.Products.Find(id).Stock -= difference;
                    _context.SaveChanges();
                    _cart[(int)id] = ItemCounter;
                }
                if(_cart[(int)id] == 0)
                {
                    _cart.Remove((int)id);
                }
                HttpContext.Session.Set("ShoppingCart", _cart);
                /*
                for (int i = 0; i < _cart.Count(); i++)
                {
                    _currentKey = keyList[i];
                    _shoppingcartVM._productcart.Add(_context.Products.Find(_currentKey), _cart[_currentKey]);
                }*/
            }
            return RedirectToAction("Index");
        }
        public IActionResult ClearShopCart()
        {
            Dictionary<int, int> _cart = new Dictionary<int, int>();
            _cart = HttpContext.Session.Get<Dictionary<int, int>>("ShoppingCart");
            List<int> keyList;
           
            if (_cart != null)
            { 
                keyList = new List<int>(_cart.Keys);
                for (int i = 0; i < _cart.Count(); i++)
                {
                    _context.Products.Find(keyList[i]).Stock += _cart[keyList[i]];
                }
            }
            HttpContext.Session.Set("ShoppingCart", new Dictionary<int, int>());
            return RedirectToAction("Index");
        }
        public IActionResult OrderFinalization()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NewOrderFinalization(CustomerOrderVM customer)
        {
            List<int> _keys;
            List<Order> _orders;
            List<Product> _orderedproducts;
            if(!ModelState.IsValid)
            {
                return View("OrderFinalization", customer);
            }

            if (HttpContext.Session.Get<Dictionary<int,int>>("ShoppingCart") != null && HttpContext.Session.Get<Dictionary<int,int>>("ShoppingCart").Count != 0)
            {
                Dictionary<int,int> _cart = new Dictionary<int, int>();
               
                _cart = HttpContext.Session.Get<Dictionary<int,int>>("ShoppingCart");
                _keys = new List<int>(_cart.Keys);
                _orders = new List<Order>();
                _orderedproducts = new List<Product>();

                foreach (int productid in _keys)
                {
                    _orders.Add(new Order() { Product =  _context.Products.Find(productid), ModelNumber = productid});  
                }
             
                
                HttpContext.Session.Set("Cart", new Dictionary<int,int>());
            }
            else
            {
                return RedirectToAction("Index", "Categories");
            }

            Customer c = new Customer()
            {
                CustomerAddress = customer.CustomerAddress,
                CustomerEmail = customer.CustomerEmail,
                CustomerName = customer.CustomerName,
                Orders = _orders
              
            };

           
            try
            {
                _context.Customers.Add(c);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "A foglalás rögzítése sikertelen, kérem próbálja újra!");
            }

            
            return RedirectToAction("Index", "Categories");
        }


    }
}