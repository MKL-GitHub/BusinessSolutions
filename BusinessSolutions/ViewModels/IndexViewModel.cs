using BusinessSolutions.Data;
using BusinessSolutions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessSolutions.ViewModels;

public class IndexViewModel
{
    public IndexViewModel(BusinessSolutionsContext context, OrderFilterViewModel filter)
    {
        Filter = filter;
        OrderNumbers = new(context.Orders.Select(p => p.Number).Distinct());
        ProviderNames = new(context.Providers.Select(p => p.Name));

        foreach (var order in context.Orders.OrderByDescending(p => p.Date))
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
                    Orders.Add(order);
                }
            }
        }
    }

    public List<Order> Orders { get; set; } = new();

    public SelectList OrderNumbers { get; set; }

    public SelectList ProviderNames { get; set; }

    public OrderFilterViewModel Filter { get; set; }
}
