using BusinessSolutions.Data;
using BusinessSolutions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessSolutions.ViewModels;

public class IndexViewModel
{
    public IndexViewModel(BusinessSolutionsContext context, IEnumerable<Order> orders, OrderFilterViewModel? filter = null)
    {
        Orders = orders;
        OrderNumbers = new(context.Orders.Select(p => p.Number).Distinct());
        ProviderNames = new(context.Providers.Select(p => p.Name));
        Filter = filter != null ? filter : new();
    }

    public IEnumerable<Order> Orders { get; set; } = new List<Order>();

    public SelectList OrderNumbers { get; set; }

    public SelectList ProviderNames { get; set; }

    public OrderFilterViewModel Filter { get; set; }
}
