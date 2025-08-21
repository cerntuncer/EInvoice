using PresentationLayer.Models.ApiResponses;

namespace PresentationLayer.Models
{
	public class DashboardProfileViewModel
	{
		// Dashboard data only
		public List<InvoiceSummaryItem> Invoices { get; set; } = new();
		public List<CurrentWithDetailsItem> Currents { get; set; } = new();

		public decimal TotalPurchaseAmount { get; set; }
		public decimal TotalSalesAmount { get; set; }
	}
}
