using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace TodoList2.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Items")]
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly MvcWebShopContext _context;

        public ItemsController(MvcWebShopContext context)
        {
            _context = context;
        }

        // GET: api/Items/?listId=5
        [HttpGet]
        public IActionResult GetItems([FromQuery] int CategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = _context.Products.Where(i => i.CategoryId == CategoryId);
            if(items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        [HttpGet("Plus/{ModelNumber}")]
        public IActionResult PlusProductNumber([FromRoute] int ModelNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _context.Products.Find(ModelNumber);
            if(item == null)
            {
                return NotFound();
            }
            Console.WriteLine("\nSZIA!" + item.Stock + "\n");
            if(item.Avalaible)
           {
                _context.Products.Find(ModelNumber).Stock += 1;
                _context.SaveChanges();
            }
           var items = _context.Products.Where(i => i.CategoryId == item.CategoryId);
            if(items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }
        [HttpGet("Minus/{DescModelNumber}")]
        public IActionResult MinusProductNumber([FromRoute] int DescModelNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _context.Products.Find(DescModelNumber);
            if(item == null)
            {
                return NotFound();
            }
            if (item.Stock > 0 && item.Avalaible)
            {
                _context.Products.Find(DescModelNumber).Stock -= 1;
                _context.SaveChanges();
            }
            var items = _context.Products.Where(i => i.CategoryId == item.CategoryId);
            if(items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpGet("Enable/{SetEnableModelNumber}")]
        public IActionResult EnableProductNumber([FromRoute] int SetEnableModelNumber)
        {
            var item = _context.Products.Find(SetEnableModelNumber);
            if (item == null)
            {
                return NotFound();
            }
            if (_context.Products.Find(SetEnableModelNumber).Avalaible)
            {
                _context.Products.Find(SetEnableModelNumber).Avalaible = false;
            }
            else { _context.Products.Find(SetEnableModelNumber).Avalaible = true; }
           // = !_context.Products.Find(SetEnableModelNumber).Avalaible;
            _context.SaveChanges();
            var items = _context.Products.Where(i => i.CategoryId == item.CategoryId);
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        // GET: api/Items/5?name=asd
        [HttpGet("{id}/{name}")]
        public IActionResult GetItem([FromRoute] int id)
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

            return Ok(item);
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

        // POST: api/Items
        [HttpPost]
        public IActionResult PostItem([FromBody] Product item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(item);
            _context.SaveChanges();

            return CreatedAtAction("GetItem", new {id = item.ModelNumber}, item);
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