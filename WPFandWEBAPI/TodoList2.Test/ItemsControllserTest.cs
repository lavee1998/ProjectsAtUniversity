using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Persistence;
using Persistence.DTOs;
using Xunit;

namespace TodoList2.WebApi.Tests
{
    public class ItemsControllserTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public ItemsControllserTest(ServerClientFixture fixture)
        {
            _fixture = fixture;
            var a = new DateTime(2019, 12, 24);
            var _orders = new List<Order>();
            _orders.Add(new Order() { Product = _fixture.Context.Products.Find(1), ModelNumber = 1, Counter = 1 });
            _orders.Add(new Order() { Product = _fixture.Context.Products.Find(2), ModelNumber = 2, Counter = 2 });
            fixture.Context.Customers.Add( new Customer()
            {
                CustomerAddress = "Budapest,Határ utca 6/2",
                CustomerEmail = "kovacslevi123@gmail.com",
                CustomerName = "Kovács Levente",
                Date = new DateTime(2019,12,24),
                CustomerPhoneNumber = "06309083111",
                Orders = _orders

            });
        }
        

        [Fact]
        public async void Test_GetCategoryItems_WithInvalidId_ReturnsEmptyList()
        {
            // Arrange
            int CategoryId = 66666;

            // Act
            var response = await _fixture.Client.GetAsync("api/Items/?CategoryId=" + CategoryId);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString);
           
            Assert.NotNull(responseObject);
            Assert.False(responseObject.Any());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void Test_GetCategoryItems_ReturnsAllRelevantItems(int listId)
        {
            // Act
            var response = await _fixture.Client.GetAsync("api/Items/?CategoryId=" + listId);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString);

            Assert.NotNull(responseObject);
            Assert.All(responseObject, item => Assert.Equal(listId, item.CategoryId));
            Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void PlusProductNumberTest(int ModelNumber)
        {
            //var item1 = _fixture.Context.Products.Find(ModelNumber);
            var response2 =await _fixture.Client.GetAsync("api/Items/?CategoryId=" + 1);
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseObject2 = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString2);
            var item1 = responseObject2.Where(p => p.ModelNumber == ModelNumber).ToList().First();
           
            var response = await _fixture.Client.GetAsync($"api/Items/Plus/{ModelNumber}");
            // Act

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString);
            var item3 = responseObject.Where(p => p.ModelNumber == ModelNumber).ToList().First();
          
            Assert.NotNull(responseObject);
            if(item3.Avalaible)
            {
                Assert.Equal(item1.Stock, item3.Stock-1);
            }
            else
            {
                Assert.Equal(item1.Stock, item3.Stock);
            }
        
          //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void MinusProductNumberTest(int DescModelNumber)
        {

            var response2 = await _fixture.Client.GetAsync("api/Items/?CategoryId=" + 1);
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseObject2 = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString2);
            var item1 = responseObject2.Where(p => p.ModelNumber == DescModelNumber).ToList().First();

          //  var item1 = _fixture.Context.Products.Find(DescModelNumber);
            // Act
            var response = await _fixture.Client.GetAsync($"api/Items/Minus/{DescModelNumber}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString);
            var item3 = responseObject.Where(p => p.ModelNumber == DescModelNumber).ToList().First();
          
            Assert.NotNull(responseObject);
            if(item3.Avalaible)
            {
                Assert.Equal(item1.Stock, item3.Stock + 1);
            }
            else
            {
                Assert.Equal(item1.Stock, item3.Stock);
            }
          
            //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }

        [Fact]
        public async void GetOrdersNumberTest()
        {
           
            // Act
            var response = await _fixture.Client.GetAsync($"api/Orders/");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString);
           
            Assert.NotNull(responseObject);
                   
            //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }


        [Theory]
        [InlineData(1)]
        public async void FinalizationDataTest(int Id)
        {
            var response2 = await _fixture.Client.GetAsync($"api/Orders/");

            // Assert
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseObject2 = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString2);
            var item1 = responseObject2.Where(w => w.CustomerId == Id);

            // Act
            var response = await _fixture.Client.GetAsync($"api/Orders/Finalization/{Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString);
            var item2 = responseObject.Where(w => w.CustomerId == Id);
            Assert.NotNull(responseObject);
            Assert.Equal(item1.Count(),item2.Count());
            //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }

        [Fact]
        public async void SearchDateTest()
        {
            var searchdatestring = new DateTime(2099, 12, 24);
            var response2 = await _fixture.Client.GetAsync($"api/Orders/");

            // Assert
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseObject2 = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString2);
           

            // Act
            var response = await _fixture.Client.GetAsync($"api/Orders/SearchDate/{searchdatestring}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString);
            Assert.NotNull(responseObject);
            Assert.Empty(responseObject);
            //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }

        [Fact]
        public async void SearchStringTest()
        {
            string searchstring = "hello";
            var searchdatestring = new DateTime(2019, 12, 24);
            var response2 = await _fixture.Client.GetAsync($"api/Orders/");

            // Assert
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseObject2 = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString2);


            // Act
            var response = await _fixture.Client.GetAsync($"api/Orders/Search/{searchstring}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString);
            Assert.NotNull(responseObject);
            Assert.Empty(responseObject);
            
            //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }

        [Fact]
        public async void IsItCompletedTest()
        {
           
            bool b = true;
            var response2 = await _fixture.Client.GetAsync($"api/Orders/");

            // Assert
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseObject2 = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString2);


            // Act
            var response = await _fixture.Client.GetAsync($"api/Orders/Product/{b}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseString);
            
            Assert.NotNull(responseObject);
            Assert.All(responseObject,item => Assert.Equal(b, item.IsItCompleted));

            //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }


        [Theory]
        [InlineData(1)]
        public async void SetEnableProductNumberTest(int SetEnableModelNumber)
        {
            var response2 = await _fixture.Client.GetAsync("api/Items/?CategoryId=" + 1);
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var responseObject2 = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString2);
            var item1 = responseObject2.Where(p => p.ModelNumber == SetEnableModelNumber).ToList().First();
           // var item1 = _fixture.Context.Products.Find(SetEnableModelNumber);
            // Act
            var response = await _fixture.Client.GetAsync($"api/Items/Enable/{SetEnableModelNumber}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<Product>>(responseString);
            var item2 = _fixture.Context.Products.Find(SetEnableModelNumber);
            var item3 = responseObject.Where(p => p.ModelNumber == SetEnableModelNumber).ToList().First();
            Assert.NotNull(responseObject);

            Assert.Equal(item1.Avalaible, !item3.Avalaible);
            //  Assert.Equal(_fixture.Context.Products.Count(i => i.CategoryId == listId), responseObject.Count());
        }




    }
}