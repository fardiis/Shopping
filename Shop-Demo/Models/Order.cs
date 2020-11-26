using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop_Demo.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public int Sum { get; set; }
        public bool IsFinaly { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }

    }

}
