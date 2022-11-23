using BusinessSolutions.Data;
using BusinessSolutions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BusinessSolutions.ViewModels;

public class DetailsViewModel
{
    public DetailsViewModel(BusinessSolutionsContext context, int id, OrderItemFilterViewModel filter)
    {
        Order = context.Orders.First(p => p.Id == id);
        Filter = filter;
        OrderItemNames = new(Order.OrderItems.Select(p => p.Name).Distinct());
        OrderItemUnits = new(Order.OrderItems.Select(p => p.Unit).Distinct());

        foreach (var orderItem in Order.OrderItems)
        {
            if (filter.OrderItemNames == null && filter.OrderItemUnits == null ||
                filter.OrderItemNames != null && filter.OrderItemNames.Contains(orderItem.Name) ||
                filter.OrderItemUnits != null && filter.OrderItemUnits.Contains(orderItem.Unit))
            {
                FilteredOrderItems.Add(orderItem);
            }
        }
    }

    public Order Order { get; set; }

    public List<OrderItem> FilteredOrderItems { get; set; } = new();

    public SelectList OrderItemNames { get; set; }

    public SelectList OrderItemUnits { get; set; }

    public OrderItemFilterViewModel Filter { get; set; }
}
