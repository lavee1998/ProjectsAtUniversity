using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DTOs
{
    public class OrderDTO
    {


        public Int32 Id { get; set; }

        public DateTime Date { get; set; }

        public String CustomerName { get; set; }

         public String CustomerAddress { get; set; }

        public String CustomerEmail { get; set; }

        public String CustomerPhoneNumber { get; set; }


        public String Description { get; set; }

        public int ModelNumber { get; set; }

        public Boolean IsItCompleted { get; set; }

        public int CustomerId { get; set; }
    }
}
