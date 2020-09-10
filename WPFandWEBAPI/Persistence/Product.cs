using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Persistence
{
    public class Product
    {
        [Required]
        [DisplayName("Gyártó")]
        public string Manufacturer { get; set; }
        [Key]
        [Required] public int ModelNumber { get; set; }
        [Required] [DisplayName("Leírás")] public string Description { get; set; }

        [Required] public Category Category { get; set; }

        [Required] [DisplayName("Nettó ár")]  public int NetPrice { get; set; }

        [Required] [DisplayName("Raktáron (db)")] public int Stock { get; set; }

        [Required] [DisplayName("Virtuális Raktár (db)")] public int VirtualStock { get; set; }

        [Required] [DisplayName("Elérhetőség")] public bool Avalaible { get; set; }

        [Required] [DisplayName("Category ID")] public int CategoryId { get; set; }



    }
}
