using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Persistence
{
    public class Category
    {
        [Required]
        public string CategoryName { get; set; }
        [Required]
        [Key]
        public int _categoryid { get; set; }
        [Required]
        public ICollection<Product> Products { get; set; }



    }

}
