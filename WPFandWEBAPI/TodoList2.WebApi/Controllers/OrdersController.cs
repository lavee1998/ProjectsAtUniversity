
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.DTOs;

namespace TodoList2.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Orders")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly MvcWebShopContext _context;

        public OrdersController(MvcWebShopContext context)
        {
            _context = context;
        }

        // GET: api/Items/?listId=5
        [HttpGet]
        public IActionResult GetOrders()
        {
           
            var items = _context.Orders.Include(b => b.Customer).Include(p => p.Product).ToList()
                .Select(order => new OrderDTO
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerAddress = order.Customer.CustomerAddress,
                    CustomerEmail = order.Customer.CustomerEmail,
                    CustomerName = order.Customer.CustomerName,
                    Description = order.Product.Description,
                    ModelNumber = order.Product.ModelNumber,
                    IsItCompleted = order.IsItCompleted,
                    CustomerPhoneNumber = order.Customer.CustomerPhoneNumber
                   
                }) ;
            foreach (var item in items)
            {
                Console.Write(item.Description + "\n");
            }
            return Ok(items);
        }

        [HttpGet("{id}")]
        public IActionResult GetItems([FromRoute] int Id)
        {
            var items = _context.Orders.Where(w => w.CustomerId == Id);
            return Ok(items);
        }

        [HttpGet("Search/{searchstring}")]
        public IActionResult SearchNameItem([FromRoute] string searchstring)
        {
            var items = _context.Orders.Include(p => p.Customer).Include(p => p.Product).Where(p => p.Customer.CustomerName.Contains(searchstring)).ToList()
               .Select(order => new OrderDTO
               {
                   Id = order.Id,
                   CustomerId = order.CustomerId,
                   CustomerAddress = order.Customer.CustomerAddress,
                   CustomerEmail = order.Customer.CustomerEmail,
                   CustomerName = order.Customer.CustomerName,
                   Description = order.Product.Description,
                   ModelNumber = order.Product.ModelNumber,
                   IsItCompleted = order.IsItCompleted,
                   Date = order.Customer.Date,
                   CustomerPhoneNumber = order.Customer.CustomerPhoneNumber

               });
            if(items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        [HttpGet("Finalization/{Id}")]
        public IActionResult FinalizationItem([FromRoute] int Id)
        {
            var list = _context.Orders.Include(p => p.Customer).Include(p => p.Product).Where(p => p.CustomerId == Id);
            if(list != null)
            {
                foreach (var item in list)
                {
                    item.Product.Stock = item.Product.VirtualStock;
                    item.IsItCompleted = true;
                }
            }
            _context.SaveChanges();
           
            var items = _context.Orders.Include(p => p.Customer).Include(p => p.Product).ToList()
               .Select(order => new OrderDTO
               {
                   Id = order.Id,
                   CustomerId = order.CustomerId,
                   CustomerAddress = order.Customer.CustomerAddress,
                   CustomerEmail = order.Customer.CustomerEmail,
                   CustomerName = order.Customer.CustomerName,
                   Description = order.Product.Description,
                   ModelNumber = order.Product.ModelNumber,
                   IsItCompleted = order.IsItCompleted,
                   Date = order.Customer.Date,
                   CustomerPhoneNumber = order.Customer.CustomerPhoneNumber

               });
            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpGet("SearchDate/{searchdatestring}")]
        public IActionResult SearchDateItem([FromRoute] DateTime searchdatestring)
        {
           
            var items = _context.Orders.Include(p => p.Customer).Include(p => p.Product).Where(p => p.Customer.Date.Year == searchdatestring.Year && p.Customer.Date.Month == searchdatestring.Month
            && p.Customer.Date.Day == searchdatestring.Day).ToList()
               .Select(order => new OrderDTO
               {
                   Id = order.Id,
                   CustomerId = order.CustomerId,
                   CustomerAddress = order.Customer.CustomerAddress,
                   CustomerEmail = order.Customer.CustomerEmail,
                   CustomerName = order.Customer.CustomerName,
                   Description = order.Product.Description,
                   ModelNumber = order.Product.ModelNumber,
                   IsItCompleted = order.IsItCompleted,
                   Date = order.Customer.Date,
                   CustomerPhoneNumber = order.Customer.CustomerPhoneNumber

               });
            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }


        // GET: api/Items/5?name=asd
        [HttpGet("Product/{b}")]
        public IActionResult GetItem([FromRoute] bool b)
        {
            var items = _context.Orders.Where(w => w.IsItCompleted == b).Include(p => p.Customer).Include(p => p.Product).ToList()
               .Select(order => new OrderDTO
               {
                   Id = order.Id,
                   CustomerId = order.CustomerId,
                   CustomerAddress = order.Customer.CustomerAddress,
                   CustomerEmail = order.Customer.CustomerEmail,
                   CustomerName = order.Customer.CustomerName,
                   Description = order.Product.Description,
                   ModelNumber = order.Product.ModelNumber,
                   IsItCompleted = order.IsItCompleted,
                   CustomerPhoneNumber = order.Customer.CustomerPhoneNumber

               });
            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
          


        }



        // PUT: api/Items/5
        [HttpPut("{id}")]
        public IActionResult PutItem([FromRoute] int id, [FromBody] Product item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.ModelNumber)
            {
                return BadRequest();
            }

            _context.Update(item);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public IActionResult DeleteItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _context.Products.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Products.Remove(item);
            _context.SaveChanges();

            return Ok(item);
        }

        private bool ItemExists(int id)
        {
            return _context.Products.Any(e => e.ModelNumber == id);
        }
    }
}