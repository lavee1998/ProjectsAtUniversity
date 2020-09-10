using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Persistence
{
    public class Order
    {
        [Key] public int Id { get; set; }

       
      
        [Required] public Product Product { get; set; }

        [Required] public int CustomerId { get; set; }

        public  Customer Customer { get; set; }

        [Required] public int ModelNumber { get; set; }

        public int Counter { get; set; }

        public Boolean IsItCompleted { get; set; }


    }
}
