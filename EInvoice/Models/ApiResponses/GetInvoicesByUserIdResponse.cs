namespace PresentationLayer.Models.ApiResponses
{
    public class GetInvoicesByUserIdResponse
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public List<InvoiceSummaryItem> Items { get; set; } = new();
    }

    public class InvoiceSummaryItem
    {
        public long Id { get; set; }
        public int Type { get; set; } // 1=Purchase, 2=Sales
        public int Senario { get; set; }
        public DateTime Date { get; set; }
        public int No { get; set; }
        public long CurrentId { get; set; }
        public long CustomerSupplierId { get; set; }
        public int Status { get; set; }
        public int LineCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}