using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebShopApplication.Models
{
    public class Colleague
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [Key]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
