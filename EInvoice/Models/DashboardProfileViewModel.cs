using System.ComponentModel.DataAnnotations;
using PresentationLayer.Models.ApiResponses;

namespace PresentationLayer.Models
{
	public class DashboardProfileViewModel
	{
		public long UserId { get; set; }
		public int UserType { get; set; }
		public int UserStatus { get; set; }

		public long PersonId { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public long IdentityNumber { get; set; }

		public string TaxOffice { get; set; }

		public int PersonType { get; set; }
		public int PersonStatus { get; set; }

		// Dashboard data
		public List<InvoiceSummaryItem> Invoices { get; set; } = new();
		public List<CurrentWithDetailsItem> Currents { get; set; } = new();

		public decimal TotalPurchaseAmount { get; set; }
		public decimal TotalSalesAmount { get; set; }
	}
}
