using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace TodoList2.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Lists")]
    public class ListsController : Controller
    {
        private readonly MvcWebShopContext _context;

        public ListsController(MvcWebShopContext context)
        {
            _context = context;
        }

        // GET: api/Lists
        [HttpGet]
        public IEnumerable<Category> GetLists()
        {
            return _context.Categories.OrderBy(l => l.CategoryName);
        }

        // GET: api/Lists/5
        [HttpGet("{id}")]
        public IActionResult GetList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = _context.Categories.Find(id);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(list);
        }

        // PUT: api/Lists/5
        [HttpPut("{id}")]
        public IActionResult PutList([FromRoute] int id, [FromBody] Category list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != list._categoryid)
            {
                return BadRequest();
            }

            _context.Update(list);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
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

        // POST: api/Lists
        [HttpPost]
        public IActionResult PostList([FromBody] Category list)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Categories.Add(list);
            _context.SaveChanges();

            return CreatedAtAction("GetList", new {id = list._categoryid}, list);
        }

        // DELETE: api/Lists/5
        [HttpDelete("{id}")]
        public IActionResult DeleteList([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var list = _context.Categories.Find(id);
            if (list == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(list);
            _context.SaveChanges();

            return Ok(list);
        }

        private bool ListExists(int id)
        {
            return _context.Categories.Any(e => e._categoryid == id);
        }
    }
}