using BusinessSolutions.Data;
using BusinessSolutions.Models;
using BusinessSolutions.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BusinessSolutions.Controllers
{
    public class OrderController : Controller
    {
        private readonly BusinessSolutionsContext _context;

        public OrderController(BusinessSolutionsContext context)
        {
            _context = context;

            var providers = _context.Providers.ToList(); //var orderItems = _context.OrderItems.ToList();
            var orders = _context.Orders.ToList();

            foreach (Provider provider in providers)
            {
                _context.Entry(provider).Collection(p => p.Orders).Load();
            }

            foreach (Order order in orders)
            {
                _context.Entry(order).Collection(p => p.OrderItems).Load();
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            IndexViewModel indexViewModel = new(_context, _context.Orders.OrderByDescending(p => p.Date));

            return View(indexViewModel);
        }

        [HttpPost]
        public IActionResult Index(OrderFilterViewModel filter)
        {
            List<Order> orders = new();

            foreach (var order in _context.Orders.OrderByDescending(p => p.Date))
            {
                if (filter.DateFrom == null && filter.DateTo == null ||
                    filter.DateFrom == null && order.Date <= filter.DateTo ||
                    filter.DateTo == null && order.Date >= filter.DateFrom ||
                    order.Date >= filter.DateFrom && order.Date <= filter.DateTo)
                {
                    if (filter.Numbers == null && filter.ProviderNames == null ||
                        filter.Numbers != null && filter.Numbers.Contains(order.Number) ||
                        filter.ProviderNames != null && filter.ProviderNames.Contains(order.Provider.Name))
                    {
                        orders.Add(order);
                    }
                }
            }

            IndexViewModel indexViewModel = new(_context, orders, filter);

            return View(indexViewModel);
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            TempData["success"] = null;
            OrderProviderViewModel createViewModel = new(_context, id);

            return View(createViewModel);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            Order dbOrder;

            if (_context.Orders.FirstOrDefault(x => x.Id == order.Id) is Order data)
            {
                dbOrder = data;
                TempData["success"] = "Заказ успешно обновлен!";
            }
            else
            {
                dbOrder = new();
                _context.Orders.Add(dbOrder);
                TempData["success"] = "Заказ успешно добавлен!";
            }

            dbOrder.Date = order.Date;
            dbOrder.Number = order.Number;
            dbOrder.Provider = _context.Providers.First(p => p.Id == order.ProviderId);
            dbOrder.ProviderId = order.ProviderId;

            var orderItems = new List<OrderItem>();

            foreach (OrderItem item in order.OrderItems)
            {
                if (item.Name == null && item.Unit == null && item.Quantity == null) continue;

                OrderItem orderItem;
                if (dbOrder.OrderItems.FirstOrDefault(p => p.Id == dbOrder.Id) is OrderItem)
                {
                    orderItem = item;
                }
                else
                {
                    orderItem = new();
                    orderItems.Add(orderItem);
                }

                orderItem.Name = item.Name;
                orderItem.Quantity = item.Quantity;
                orderItem.Unit = item.Unit;
                orderItem.OrderId = order.Id;
                orderItem.Order = dbOrder;
            }

            dbOrder.OrderItems.Clear();

            foreach (OrderItem orderItem in orderItems)
            {
                dbOrder.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();

            OrderProviderViewModel createViewModel = new(_context, dbOrder.Id);

            return View(createViewModel);
        }

        public IActionResult Delete(int id)
        {
            if (_context.Orders.FirstOrDefault(p => p.Id == id) is Order order)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
                TempData["success"] = "Заказ успешно удален!";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int orderId)
        {
            DetailsViewModel detailsViewModel = new(_context, orderId);

            return View(detailsViewModel);
        }
    }
}
