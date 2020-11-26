using System.ComponentModel.DataAnnotations;

namespace Shop_Demo.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int OrderId { get; set; } 
        [Required]
        public int Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public Order Order { get; set; }
        public int ProductId { get; set; }
        [Required]
        public Product Product { get; set; }
       


    }
}



