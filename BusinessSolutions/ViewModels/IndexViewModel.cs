using BusinessSolutions.Models;

namespace BusinessSolutions.ViewModels;

public class IndexViewModel
{
    public IndexViewModel(IEnumerable<Order> orders, IEnumerable<Provider> providers)
    {
        Orders = orders;

        foreach (Order order in orders)
        {
            if (!string.IsNullOrEmpty(order.Number))
            {
                OrderNumbers.Add(order.Number);
            }
        }

        foreach (Provider provider in providers)
        {
            if (!string.IsNullOrEmpty(provider.Name))
            {
                ProviderNames.Add(provider.Name);
            }
        }
    }

    public IEnumerable<Order> Orders { get; set; } = new List<Order>();

    public List<string> OrderNumbers { get; set; } = new();

    public List<string> ProviderNames { get; set; } = new();

}
