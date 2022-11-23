using BusinessSolutions.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessSolutions.ViewModels;

public class OrderItemFilterViewModel
{
    public IEnumerable<string>? OrderItemNames { get; set; }

	public IEnumerable<string>? OrderItemUnits { get; set; }
}
