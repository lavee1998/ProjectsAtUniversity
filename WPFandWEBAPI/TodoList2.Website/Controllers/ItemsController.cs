using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoList2.Persistence;
using TodoList2.Website.Services;

namespace TodoList2.Website.Controllers
{
    [Authorize]
	public class ItemsController : Controller
	{
		private readonly TodoListService _todoListService;

		public ItemsController(TodoListService todoListService)
		{
		    _todoListService = todoListService;
		}

		// GET: Items/Create
		public IActionResult Create()
		{
			ViewData["Lists"] = new SelectList(_todoListService.GetLists(), "Id", "Name", TempData["ListId"]);
			return View();
		}

		// POST: Items/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Item item)
		{
			if (ModelState.IsValid)
			{
			    _todoListService.CreateItem(item);
				return RedirectToAction("Details", "Lists", new { id = item.ListId });
			}

			ViewData["Lists"] = new SelectList(_todoListService.GetLists(), "Id", "Name", item.ListId);
			return View(item);
		}

		// GET: Items/Edit/5
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			var item = _todoListService.GetItemById((int)id);
			if (item == null)
			{
				return NotFound();
			}

			ViewData["Lists"] = new SelectList(_todoListService.GetLists(), "Id", "Name", item.ListId);
			return View(item);
		}

		// POST: Items/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, Item item)
		{
			if (id != item.Id)
			{
				return BadRequest();
			}

		    if (ModelState.IsValid)
		    {
		        var result = _todoListService.UpdateItem(item);
		        switch (result)
		        {
		            case TodoListService.TodoListUpdateResult.Success:
		                return RedirectToAction("Details", "Lists", new { id = item.ListId });

		            case TodoListService.TodoListUpdateResult.ConcurrencyError:
		                if (!_todoListService.ListExists(id))
		                {
		                    return NotFound();
		                }

		                ModelState.AddModelError("", "Hiba történt a mentés során!");
		                break;

		            case TodoListService.TodoListUpdateResult.DbError:
		                ModelState.AddModelError("", "Adatbázis hiba!");
		                break;
		        }

		    }

			ViewData["Lists"] = new SelectList(_todoListService.GetLists(), "Id", "Name", item.ListId);
			return View(item);
		}

		// GET: Items/Delete/5
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}

		    var item = _todoListService.GetItemById((int)id, true);
			if (item == null)
			{
				return NotFound();
			}

			return View(item);
		}

		// POST: Items/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
		    var item = _todoListService.GetItemById(id);
		    if (item != null)
		    {
		        _todoListService.DeleteItem(id);
		        return RedirectToAction("Details", "Lists", new { id = item.ListId });
		    }

		    return NotFound ();
		}
	}
}
