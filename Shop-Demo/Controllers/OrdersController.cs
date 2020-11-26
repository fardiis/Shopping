using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Shop_Demo.Data;
using Shop_Demo.Models;

namespace Shop_Demo.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private MyContext _ctx;
        public OrdersController(MyContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult AddToCart(int id)
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserId && !o.IsFinaly);
            if (order == null)
            {
                order = new Order()
                {
                    UserId = CurrentUserId,
                    CreateTime = DateTime.Now,
                    IsFinaly = false,
                    Sum = 0

                };
                _ctx.Orders.Add(order);
                _ctx.OrderDetails.Add(new OrderDetail()
                {
                    OrderId = order.OrderId,
                    Count = 1,
                    Price = _ctx.Products.Find(id).Price,
                    ProductId = id

                });
                _ctx.SaveChanges();
            }
            else
            {
                var details = _ctx.OrderDetails.SingleOrDefault(d => d.OrderId == order.OrderId && d.ProductId == id);
                if (details == null)
                {
                    _ctx.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        Price = _ctx.Products.Find(id).Price,
                        ProductId = id

                    });
                }
                else
                {
                    details.Count += 1;
                    _ctx.Update(details);

                }
                UpdateSumOrder(order.OrderId);
                _ctx.SaveChanges();
            }
            return Redirect("/");
        }

        public IActionResult ShowOrder()
        {
            string CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Order order = _ctx.Orders.SingleOrDefault(o => o.UserId == CurrentUserId && !o.IsFinaly);
            List<ShowOrderViewModel> _list = new List<ShowOrderViewModel>();
            if (order != null)
            {
                var details = _ctx.OrderDetails.Where(d => d.OrderId == order.OrderId).ToList();
                foreach (var item in details)
                {
                    var product = _ctx.Products.Find(item.ProductId);
                    _list.Add(new ShowOrderViewModel()
                    {
                        Count =item.Count,
                        ImageName=product.ImageName,
                        OrderDetailId=item.OrderDetailId,
                        Price=item.Price,
                        Sum=item.Count*item.Price,
                        Title=product.Title
                    });
                }
            }
            return View(_list);
        }
        public  IActionResult Delete(int id)
        {
            var orderDetail = _ctx.OrderDetails.Find(id);
            _ctx.Remove(orderDetail);
            _ctx.SaveChanges();
            return RedirectToAction("ShowOrder");
        }
        public IActionResult Command(int id,string command)
        {
            var orderDetail = _ctx.OrderDetails.Find(id);
            switch (command)
            {
                case "up":
                    {
                        orderDetail.Count += 1;
                        _ctx.Update(orderDetail);
                        break;
                    }
                case "down":
                    {
                        orderDetail.Count -= 1;
                        if (orderDetail.Count == 0)
                        {
                         _ctx.OrderDetails.Remove(orderDetail);
                        }
                            
                        else
                        {
                            _ctx.Update(orderDetail);
                        }
                        break;
                       
                    }
            }
            _ctx.SaveChanges();
            return RedirectToAction("ShowOrder");
        }
        public void UpdateSumOrder(int orderId)
        {
            var order = _ctx.Orders.Find(orderId);
            order.Sum = _ctx.OrderDetails.Where(o => o.OrderId == order.OrderId).Select(d => d.Count * d.Price).Sum();

            _ctx.Update(order);
            _ctx.SaveChanges();

        }
    }
}

