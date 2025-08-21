using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.InvoiceHandler.DTOs
{
	public class GetInvoicesByUserIdHandleResponse
	{
		public bool Error { get; set; }
		public string Message { get; set; }
		public List<InvoiceSummaryDto> Items { get; set; } = new();
	}

	public class InvoiceSummaryDto
	{
		public long Id { get; set; }
		public InvoiceType Type { get; set; }
		public InvoiceSenario Senario { get; set; }
		public DateTime Date { get; set; }
		public int No { get; set; }
		public long CurrentId { get; set; }
		public long CustomerSupplierId { get; set; }
		public Status Status { get; set; }
		public int LineCount { get; set; }
		public decimal TotalAmount { get; set; }
	}
}