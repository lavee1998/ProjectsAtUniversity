using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopApplication.Models
{
    public class Customer
    {
        [Key] public int Id { get; set; }
       
        [Required(ErrorMessage = "A felhasználónév megadása kötelező.")]
        [RegularExpression("^[A-Za-z0-9_-]{5,40}$", ErrorMessage = "A felhasználónév formátuma, vagy hossza nem megfelelő.")] public String CustomerName { get; set; }
      
        [Required(ErrorMessage = "A cím megadása kötelező.")] public String CustomerAddress { get; set; }

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Az e-mail cím nem megfelelő formátumú.")]
        [DataType(DataType.EmailAddress)] // pontosítjuk az adatok típusát
        public String CustomerEmail { get; set; }

       

        public ICollection<Order> Orders { get; set; }
    }
}
