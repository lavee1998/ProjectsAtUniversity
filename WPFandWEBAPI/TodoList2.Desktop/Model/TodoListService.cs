using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Persistence;
using Persistence.DTOs;

namespace TodoList2.Desktop.Model
{
    public class TodoListService : ITodoListService
    {
        private readonly HttpClient _client;

        private bool _isUserLoggedIn;
        public bool IsUserLoggedIn => _isUserLoggedIn;

        public TodoListService(string baseAddress)
        {
            _isUserLoggedIn = false;
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task<IEnumerable<Category>> LoadListsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("api/Lists/");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<IEnumerable<Category>>();
            }

            throw new NetworkException("Service returned response2: " + response.StatusCode);
        }

        public async Task<IEnumerable<Product>> LoadItemsAsync(int CategoryId)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Items/?CategoryId={CategoryId}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<Product>>();
            }

            throw new NetworkException("Service returned response9: " + response.StatusCode);
        }

        public async Task<IEnumerable<OrderDTO>> LoadOrdersAsync()
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Orders/");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
            }

            throw new NetworkException("Service returned response10: " + response.StatusCode);
        }

        public async Task<IEnumerable<OrderDTO>> SearchOrdersAsync(string searchstring)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Orders/Search/{searchstring}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
            }

            throw new NetworkException("Service returned response10: " + response.StatusCode);
        }


        public async Task<IEnumerable<OrderDTO>> FinalizationOrdersAsync(int Id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Orders/Finalization/{Id}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
            }

            throw new NetworkException("Service returned response10: " + response.StatusCode);
        }

        public async Task<IEnumerable<OrderDTO>> SearchDateOrdersAsync(DateTime searchdatestring)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Orders/SearchDate/{searchdatestring}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
            }

            throw new NetworkException("Service returned response10: " + response.StatusCode);
        }

        public async Task<IEnumerable<OrderDTO>> LoadProductsAsync(bool b)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Orders/Product/{b}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
            }

            throw new NetworkException("Service returned response10: " + response.StatusCode);
        }
        public async Task<IEnumerable<Product>> PlusItemsAsync(int ModelNumber)
        {
            Console.WriteLine("\nHALIMALI\n");
            HttpResponseMessage response = await _client.GetAsync($"api/Items/Plus/{ModelNumber}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<Product>>();
            }


            throw new NetworkException("Service returned response3: " + response.StatusCode);

        }
        public async Task<IEnumerable<Product>> MinusItemsAsync(int DescModelNumber)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/Items/Minus/{DescModelNumber}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<Product>>();
            }

            throw new NetworkException("Service returned response3: " + response.StatusCode);

        }
        public async Task<IEnumerable<Product>> EnableItemsAsync(Int32 SetEnableModelNumber) 
        {

            HttpResponseMessage response = await _client.GetAsync($"api/Items/Enable/{SetEnableModelNumber}");
            if (response.IsSuccessStatusCode)
            {
                // return JsonConvert.DeserializeObject<IEnumerable<Item>>(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsAsync<IEnumerable<Product>>();
            }
            throw new NetworkException("Service returned response3: " + response.StatusCode);

        }

        public async Task<bool> LoginAsync(string name, string password)
        {
            LoginDto user = new LoginDto
            {
                UserName = name,
                Password = password
            };

            //HttpResponseMessage response = await _client.PostAsync("api/Account/Login",
            //    new StringContent(JsonConvert.SerializeObject(user),
            //        Encoding.UTF8,
            //        "application/json"));
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Login", user);

            if (response.IsSuccessStatusCode)
            {
                _isUserLoggedIn = true;
                return true;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            throw new NetworkException("Service returned response4: " + response.StatusCode);
        }

        public async Task<bool> LogoutAsync()
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync("api/Account/Signout", "");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            throw new NetworkException("Service returned response: " + response.StatusCode);
        }
    }
}