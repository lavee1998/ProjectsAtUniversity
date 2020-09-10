using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class SeedData
    {

        public static void Initialize(MvcWebShopContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            /*
             Adatbázis létrehozása, amennyiben nem létezik. A változásokat nem kezeli. Ha változtattunk az
             adatbázis modelljén (a code-first osztályainkon), akkor az adatbázist törölni kell, hogy itt
             újra létrejöhessen a frissített szerkezettel.
             Törléshez az Sql Server Object Explorer ablak használható a Visual Studioban.
             Itt SQL Server > localdb > databases, vagy valami hasonló útvonalon találjuk az adatbázisainkat.
             */
           context.Database.EnsureCreated();


            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }
         

            context.Categories.AddRange(
                new Category
                {
                    CategoryName = "Tejtermék",
                    Products = new List<Product>()
                    {
                            new Product()
                            {
                            Description = "Túró",
                            Manufacturer = "Mizo",
                            NetPrice = 100,
                            Stock  = 100,
                            VirtualStock = 100,
                                Avalaible = true

                            },
                            new Product()
                            {
                                Description = "Tejföl8654",
                                Manufacturer = "Danone" ,
                                NetPrice = 120,
                                Stock = 900,
                                VirtualStock = 900,
                                Avalaible = true
                            },
                            new Product()
                            {
                                Description = "Sajt1754",
                                Manufacturer = "Trapist",
                                NetPrice = 1200,
                                Stock = 12912,
                                VirtualStock = 12912,
                                Avalaible = true
                            }
                            ,
                            new Product()
                            {
                                Description = "Sajt2os",
                                Manufacturer = "Trapist",
                                NetPrice = 900,
                                Stock = 4,
                                VirtualStock = 4,
                                Avalaible = true
                            }
                            ,
                            new Product()
                            {
                                Description = "ISajt",
                                Manufacturer = "Apple",
                                NetPrice = 900000,
                                Stock = 9,
                                VirtualStock =9,
                                Avalaible = true
                            }
                            ,
                            new Product()
                            {
                                Description = "Sajt86543",
                                Manufacturer = "Trapist",
                                NetPrice = 909,
                                Stock = 12,
                                VirtualStock = 12,
                                Avalaible = true
                            }
                            ,
                            new Product()
                            {
                                Description = "Sajt65654",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt4987",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt3434",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt784",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt498",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt984",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt445",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt46",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock =98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt84",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt47",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt98",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },

                             new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                              new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }
                                                        ,
                            new Product()
                            {
                                Description = "NINCSRAKTÁRONSAJT",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 0,
                                VirtualStock = 0,
                                Avalaible = true
                            }
                            ,
                            new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                             new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                              new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                               new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                 new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                  new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                   new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                    new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                     new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                      new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            },
                                       new Product()
                            {
                                Description = "Sajt99",
                                Manufacturer = "Eda ",
                                NetPrice = 912,
                                Stock = 98,
                                VirtualStock = 98,
                                Avalaible = true
                            }

                    }
                },

                new Category
                {
                    CategoryName = "Hústermék",
                    Products = new List<Product>()
                    {
                            new Product(){
                                Description = "NEMELÉRHETŐSzalámi",
                                Manufacturer = "Pick",
                                NetPrice = 1200,
                                Stock = 2,
                                VirtualStock = 2,
                                Avalaible = false
                            },
                            new Product()
                            {
                                Description  ="Szalonna",
                                Manufacturer = "Szalonnafeldolgozó",
                                NetPrice = 2000,
                                Stock = 10,
                                VirtualStock = 10,
                                Avalaible = true
                            },
                            new Product()
                            {
                                Description = "Párizsi1",
                                Manufacturer = "Párizsifeldolgozó",
                                NetPrice = 1700,
                                Stock = 12,
                                VirtualStock = 12,
                                Avalaible = true
                            },
                            new Product()
                            {
                                Description = "Párizsi2",
                                Manufacturer = "Párizsifeldolgozó",
                                NetPrice = 1700,
                                Stock = 12,
                                VirtualStock = 12,
                                Avalaible = true
                            },
                            new Product()
                            {
                                Description = "Párizsi2",
                                Manufacturer = "Párizsifeldolgozó",
                                NetPrice = 1700,
                                Stock = 12,
                                VirtualStock = 12,
                                Avalaible = true
                            },
                            new Product()
                            {
                                Description = "Párizsi3",
                                Manufacturer = "Párizsifeldolgozó",
                                NetPrice = 1700,
                                Stock = 12,
                                VirtualStock = 12,
                                Avalaible = true
                            },
                            new Product()
                            {
                                Description = "Párizsi4",
                                Manufacturer = "Párizsifeldolgozó",
                                NetPrice = 1700,
                                Stock = 12,
                                VirtualStock = 12,
                                Avalaible = true
                            }

                    }
                }


            );

            Task.Run(async () =>
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                var adminUser = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@xy.zz"
                };
                await userManager.CreateAsync(adminUser, "123");
                await userManager.AddToRoleAsync(adminUser, "admin");
            }).GetAwaiter().GetResult();

          

            context.SaveChanges();
        }


       
  
             
    }
}