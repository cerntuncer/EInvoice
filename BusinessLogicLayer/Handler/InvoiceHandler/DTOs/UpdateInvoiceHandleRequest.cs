using BusinessLogicLayer.Handler.LineOfInvoiceHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler.DTOs
{
    public class UpdateInvoiceHandleRequest : IRequest<UpdateInvoiceHandleResponse>
    {
        public long Id { get; set; } // Güncellenecek fatura ID
        public InvoiceType Type { get; set; }
        public InvoiceSenario Senario { get; set; }
        public long CurrentId { get; set; }
        public long CustomerSupplierId { get; set; }
        public Status Status { get; set; }
        public ICollection<lineOfInvoices> lineOfInovices { get; set; }
    }

   
}
