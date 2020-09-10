using System;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Persistence;
using Persistence.DTOs;
using TodoList2.WebApi;

namespace TodoList2.WebApi.Tests
{
    public class ServerClientFixture : IDisposable
    {
        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public MvcWebShopContext Context { get; private set; }

        public ServerClientFixture()
        {
            // Arrange
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());

            Context = Server.Host.Services.GetRequiredService<MvcWebShopContext>();
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}