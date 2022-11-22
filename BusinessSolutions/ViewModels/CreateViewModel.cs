using BusinessSolutions.Data;
using BusinessSolutions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessSolutions.ViewModels;

public class CreateViewModel
{
    public CreateViewModel(BusinessSolutionsContext context, int? orderId)
    {
        Providers = new(context.Providers.Select(x => new KeyValuePair<string, string>(x.Name, x.Id.ToString())), "Value", "Key");

        Order = context.Orders.FirstOrDefault(p => p.Id == orderId) is Order order ? order : new(); 
    }

    public Order Order { get; set; }

    public SelectList Providers { get; set; }
}
