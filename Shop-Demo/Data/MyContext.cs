using Microsoft.EntityFrameworkCore;
using Shop_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Demo.Data
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
