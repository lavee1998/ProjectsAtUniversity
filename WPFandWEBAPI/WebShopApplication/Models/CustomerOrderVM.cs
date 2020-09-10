using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Persistence;

namespace WebShopApplication.Models
{
    public class CustomerOrderVM
    {
        [Required(ErrorMessage = "A név megadása kötelező.")] // feltételek a validáláshoz
        [StringLength(40, ErrorMessage = "A feladó neve maximum 60 karakter lehet.")]
        public String CustomerName { get; set; }

        /// <summary>
        /// Vendég e-mail címe.
        /// </summary>
        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Az e-mail cím nem megfelelő formátumú.")]
        [DataType(DataType.EmailAddress)] // pontosítjuk az adatok típusát
        public String CustomerEmail { get; set; }

        /// <summary>
        /// Vendég címe.
        /// </summary>
        [Required(ErrorMessage = "A cím megadása kötelező.")]
        public String CustomerAddress { get; set; }

        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [Phone(ErrorMessage = "Az e-mail cím nem megfelelő formátumú.")]
        [DataType(DataType.PhoneNumber)]
        public String CustomerPhoneNumber { get; set; }
    }
}
