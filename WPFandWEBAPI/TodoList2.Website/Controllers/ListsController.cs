using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoList2.Persistence;
using TodoList2.Website.Services;

namespace TodoList2.Website.Controllers
{
    [Authorize]
	public class ListsController : Controller
	{
	    private readonly TodoListService _todoListService;

	    public ListsController(TodoListService todoListService)
	    {
	        _todoListService = todoListService;
	    }

		// GET: Lists
        [AllowAnonymous]
		public IActionResult Index(string searchString)
        {
            ViewBag.SearchString = searchString;
			return View(_todoListService.GetLists(searchString));
		}

		// GET: Lists/Details/5
        [AllowAnonymous]
		public IActionResult Details(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}

		    var list = _todoListService.GetListById((int)id, includeItems: true);
			if (list == null)
			{
				return NotFound();
			}

			return View(list);
		}

		// GET: Lists/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Lists/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(List list)
		{
			if (ModelState.IsValid)
			{
			    _todoListService.CreateList(list);
				return RedirectToAction(nameof(Index));
			}

			return View(list);
		}

		// GET: Lists/Edit/5
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			var list = _todoListService.GetListById((int)id);
			if (list == null)
			{
				return NotFound();
			}

			return View(list);
		}

		// POST: Lists/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, List list)
		{
			if (id != list.Id)
			{
				return BadRequest();
			}

			if (ModelState.IsValid)
			{
			    var result = _todoListService.UpdateList(list);
			    switch (result)
			    {
			        case TodoListService.TodoListUpdateResult.Success:
			            return RedirectToAction(nameof(Index));

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

			return View(list);
		}

		// GET: Lists/Delete/5
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return BadRequest();
			}

			var list = _todoListService.GetListById((int)id);
			if (list == null)
			{
				return NotFound();
			}

			return View(list);
		}

		// POST: Lists/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
		    _todoListService.DeleteList(id);
			return RedirectToAction(nameof(Index));
		}

		// GET: Lists/CreateItem/5
		public IActionResult CreateItem(int id)
		{
			TempData["ListId"] = id; // Akciók közti információátadásnál a ViewData és ViewBag nem használható.
			return RedirectToAction("Create", "Items");
		}

	}
}
