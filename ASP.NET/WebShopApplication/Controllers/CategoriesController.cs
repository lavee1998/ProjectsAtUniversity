using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShopApplication.Data;
using WebShopApplication.Models;

namespace WebShopApplication.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly MvcWebShopContext _context;

        public CategoriesController(MvcWebShopContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
           // var _list = await _context.Categories.Include(m => (m.Products.Where(k => k.Stock > 0)))
           //     .ToListAsync();
            var _list = await _context.Categories.Include(m => m.Products)
                 .ToListAsync();
          
          
            return View(_list);

        }

        
     
        public async Task<IActionResult> Details(int? id , int pagenumber = 0 , string OrderString = "1", string OrderByPriceString = "1" )
        {
            ViewBag.pagenumber = pagenumber;
            ViewBag.ascString = OrderString;
            ViewBag.OrdByPrice = OrderByPriceString;
           
            var selectitems_helper = new List<Tuple<string, int>>() {
            new Tuple<string,int>("Ár szerinti rendezés",1),
            new Tuple<string,int>("Gyártó szerinti rendezés",0)};
               

                var selectitems1 = selectitems_helper.Select(n => new SelectListItem
                {
                    Value = n.Item2.ToString(),
                    Text = n.Item1.ToString()
                }).ToList();

            selectitems_helper = new List<Tuple<string, int>>() {
            new Tuple<string,int>("Növekvő",1),
            new Tuple<string,int>("Csökkenő",0)};


            var selectitems2 = selectitems_helper.Select(n => new SelectListItem
            {
                Value = n.Item2.ToString(),
                Text = n.Item1.ToString()
            }).ToList();


            if (id == null)
            {
                return NotFound();
            }
            var products = _context.Products.Include(i => i.Category).Where(p => p.CategoryId == id && p.Stock >0 && p.Avalaible);
            var category =  await _context.Categories
                .Include(i => i.Products).FirstOrDefaultAsync(m => m._categoryid == id);
            
            if (category == null)
            {
                return NotFound();
            }
            if(OrderString == "1")
            {   
                if(OrderByPriceString == "1")
                {
                    products = products.OrderBy(o => o.NetPrice);
                }
                else
                {
                    products = products.OrderBy(o => o.Manufacturer);
                }
            }
            else
            {
                 if (OrderByPriceString == "1")
                 {
                    products = products.OrderByDescending(o => o.NetPrice);
                 }
                    else
                    {
                        products = products.OrderByDescending(o => o.Manufacturer);
                    }
            }
            ViewBag.productCounter = products.Count();
            OrderViewModel OrderViewModel = new OrderViewModel()
            {
                ProductsVM = products.Skip(pagenumber * 20).Take(20).ToList(),
                OrderByPriceList = (List<SelectListItem>)selectitems1,
                OrdersList = (List<SelectListItem>)selectitems2

            };

            return View(OrderViewModel);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,_categoryid")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryName,_categoryid")] Category category)
        {
            if (id != category._categoryid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category._categoryid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m._categoryid == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e._categoryid == id);
        }

        public IActionResult AddProductToCart(int? id, int? productid, int ItemCounter = 1)
        {
           

            if (!(_context.Products.Any(e => e.ModelNumber == (int)productid )) && ItemCounter < 0)
            {
                return BadRequest();
            }
            if (_context.Products.Find((int)productid).Stock >= ItemCounter && _context.Products.Find((int)productid).Avalaible)
            {


                ViewBag.ItemCounter = ItemCounter;
                Dictionary<int, int> _ShoppingCart = new Dictionary<int, int>();
                if (HttpContext.Session.Get<Dictionary<int, int>>("ShoppingCart") != null)
                {
                    _ShoppingCart = HttpContext.Session.Get<Dictionary<int, int>>("ShoppingCart");
                }
                else
                {
                    HttpContext.Session.Set("ShoppingCart", new Dictionary<int, int>());
                }


                if (_ShoppingCart.ContainsKey((int)productid))
                {
                    _ShoppingCart[(int)productid] = _ShoppingCart[(int)productid] + ItemCounter;
                }
                else
                {
                    _ShoppingCart.Add((int)productid, ItemCounter);
                }
             _context.Products.Find((int)productid).Stock -= ItemCounter;
            _context.SaveChanges();
            HttpContext.Session.Set("ShoppingCart", _ShoppingCart);

            }
            
          
            return RedirectToAction("Details", new { id = (int)id , pagenumber = ViewBag.pagenumber , OrderString = ViewBag.ascString , OrderByPriceString = ViewBag.OrderByPrice});
        }
    }
}
