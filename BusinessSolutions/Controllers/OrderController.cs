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

            foreach (Order order in _context.Orders.ToList())
            {
                _context.Entry(order).Collection(p => p.OrderItems).Load();
            }

            foreach (Provider provider in _context.Providers.ToList())
            {
                _context.Entry(provider).Collection(p => p.Orders).Load();
            }
        }

        public IActionResult Index(OrderFilterViewModel filter)
        {
            IndexViewModel indexViewModel = new(_context, filter);

            return View(indexViewModel);
        }

        [HttpGet]
        public IActionResult Create(int? id)
        {
            resetNotificationData();
            CreateViewModel createViewModel = new(_context, id);

            return View(createViewModel);
        }

        [HttpPost]
        public IActionResult Create(Order order)
        {
            resetNotificationData();

            if (!IsValidateOrder(order))
            {
                CreateViewModel createViewModelMistake = new(_context, order);
                return View(createViewModelMistake);
            }


            Order dbOrder;

            if (_context.Orders.FirstOrDefault(x => x.Id == order.Id) is Order data)
            {
                dbOrder = data;
                TempData["successHead"] = "Успешно!";
                TempData["success"] = "Заказ обновлен";
            }
            else
            {
                dbOrder = new();
                _context.Orders.Add(dbOrder);
                TempData["successHead"] = "Успешно!";
                TempData["success"] = "Заказ добавлен";
            }

            dbOrder.Date = order.Date;
            dbOrder.Number = order.Number;
            dbOrder.Provider = _context.Providers.First(p => p.Id == order.ProviderId);
            dbOrder.ProviderId = order.ProviderId;

            List<OrderItem> orderItems = new();

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

            CreateViewModel createViewModel = new(_context, dbOrder.Id);

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

        public IActionResult Details(int id, OrderItemFilterViewModel filter)
        {
            DetailsViewModel detailsViewModel = new(_context, id, filter);

            return View(detailsViewModel);
        }

        private void resetNotificationData()
        {
            TempData["successHead"] = null;
            TempData["errorHead"] = null;
            TempData["success"] = null;
            TempData["error"] = null;
        }

        private bool IsValidateOrder(Order order)
        {
            foreach (var item in _context.Orders)
            {
                if (item.Number == order.Number && item.ProviderId == order.ProviderId && order.Id != item.Id)
                {
                    TempData["errorHead"] = "Ошибка!";
                    TempData["error"] = "Уже есть заказ с таким номером и поставщиком";

                    return false;
                }
            }

            foreach (var item in order.OrderItems)
            {
                if (item.Name == order.Number)
                {
                    TempData["errorHead"] = "Ошибка!";
                    TempData["error"] = "Название элемента заказа не должно совпадать с номером заказа";

                    return false;
                }
            }

            return true;
        }
    }
}
