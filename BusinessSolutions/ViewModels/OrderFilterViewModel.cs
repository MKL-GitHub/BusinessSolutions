namespace BusinessSolutions.ViewModels;

public class OrderFilterViewModel
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public IEnumerable<string>? Numbers { get; set; }

	public IEnumerable<string>? ProviderNames { get; set; }
}
