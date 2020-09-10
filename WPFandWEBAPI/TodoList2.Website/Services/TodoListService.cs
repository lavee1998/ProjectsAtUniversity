using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoList2.Persistence;

namespace TodoList2.Website.Services
{
    public class TodoListService
    {
        private readonly TodoListDbContext _context;

        public enum TodoListUpdateResult
        {
            Success,
            ConcurrencyError,
            DbError
        }

        #region Lists
        public TodoListService(TodoListDbContext context)
        {
            _context = context;
        }

        public List<List> GetLists(string searchString = null)
        {
            return _context.Lists
                .Where(l => l.Name.Contains(searchString ?? ""))
                .OrderBy(l => l.Name)
                .ToList();
        }

        public List GetListById(int id, bool includeItems = false)
        {
            if (includeItems)
            {
                return _context.Lists
                    .Include(m => m.Items)
                    .FirstOrDefault(m => m.Id == id);
            }
            else
            {
                return _context.Lists.Find(id);
            }
        }

        public bool CreateList(List list)
        {
            try
            {
                _context.Add(list);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public TodoListUpdateResult UpdateList(List list)
        {
            try
            {
                _context.Update(list);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return TodoListUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return TodoListUpdateResult.DbError;
            }

            return TodoListUpdateResult.Success;
        }

        public bool ListExists(int id)
        {
            return _context.Lists.Any(e => e.Id == id);
        }

        public bool DeleteList(int id)
        {
            var list = _context.Lists.Find(id);
            if (list == null)
                return false;

            try
            {
                _context.Lists.Remove(list);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Items
        public List<Item> GetItems()
        {
            return _context.Items.OrderBy(l => l.Name).ToList();
        }

        public Item GetItemById(int id, bool includeList = false)
        {
            if (includeList)
            {
                return _context.Items
                    .Include(m => m.List)
                    .FirstOrDefault(m => m.Id == id);
            }
            else
            {
                return _context.Items.Find(id);
            }
        }

        public bool CreateItem(Item item)
        {
            try
            {
                _context.Add(item);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }

        public TodoListUpdateResult UpdateItem(Item item)
        {
            try
            {
                _context.Update(item);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return TodoListUpdateResult.ConcurrencyError;
            }
            catch (DbUpdateException)
            {
                return TodoListUpdateResult.DbError;
            }

            return TodoListUpdateResult.Success;
        }

        public bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        public bool DeleteItem(int id)
        {
            var item = _context.Items.Find(id);
            if (item == null)
                return false;

            try
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
