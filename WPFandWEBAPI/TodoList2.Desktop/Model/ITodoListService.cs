using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Persistence.DTOs;

namespace TodoList2.Desktop.Model
{
    public interface ITodoListService
    {
        bool IsUserLoggedIn { get; }
        Task<IEnumerable<Category>> LoadListsAsync();
        Task<IEnumerable<Product>> LoadItemsAsync(Int32 listId);

        Task<IEnumerable<OrderDTO>> LoadOrdersAsync();

        Task<IEnumerable<OrderDTO>> FinalizationOrdersAsync(int Id);
        Task<IEnumerable<OrderDTO>> SearchOrdersAsync(string searchstring);

        Task<IEnumerable<OrderDTO>> SearchDateOrdersAsync(DateTime searchDatestring);


        Task<IEnumerable<OrderDTO>> LoadProductsAsync(bool b);

        Task<IEnumerable<Product>>PlusItemsAsync(Int32 ModelNumber);
        Task<IEnumerable<Product>> MinusItemsAsync(Int32 DescModelNumber);

        Task<IEnumerable<Product>> EnableItemsAsync(Int32 SetEnableModelNumber);


        Task<bool> LoginAsync(string name, string password);
        Task<bool> LogoutAsync();
    }
}