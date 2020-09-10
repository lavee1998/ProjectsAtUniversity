using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace TodoList2.WebApi.Tests
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MvcWebShopContext>(options =>
                options.UseInMemoryDatabase("TestingDB"));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MvcWebShopContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MvcWebShopContext context)
        {
            app.UseAuthentication();
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>(); // automatikus "bejelentkezés"

            app.UseMvc();
            TestDbInitializer.Initialize(context);
        }
    }
}