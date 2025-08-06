using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Handler.InvoiceHandler
{
    public class GetInvoiceByIdHandle : IRequestHandler<GetInvoiceByIdHandleRequest, GetInvoiceByIdHandleResponse>
    {
        private readonly MyContext _context;

        public GetInvoiceByIdHandle(MyContext context)
        {
            _context = context;
        }

        public async Task<GetInvoiceByIdHandleResponse> Handle(GetInvoiceByIdHandleRequest request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices
                .Include(i => i.LineOfInvoices)
                .FirstOrDefaultAsync(i => i.Id == request.Id);

            if (invoice == null)
            {
                return new GetInvoiceByIdHandleResponse
                {
                    Error = true,
                    Message = "Fatura bulunamadı."
                };
            }

            return new GetInvoiceByIdHandleResponse
            {
                Error = false,
                Message = "Fatura başarıyla getirildi.",
                Id = invoice.Id,
                Type = invoice.Type,
                Senario = invoice.Senario,
                Date = invoice.Date,
                No = invoice.No,
                CurrentId = invoice.CurrentId,
                CustomerSupplierId = invoice.CustomerSupplierId,
                Status = invoice.Status,
                Lines = invoice.LineOfInvoices.Select(line => new LineOfInvoiceDto
                {
                    Id = line.Id,
                    InvoiceId = line.InvoiceId,
                    ProductAndServiceId = line.ProductAndServiceId,
                    Quantity = line.Quantity,
                    UnitPrice = line.UnitPrice
                }).ToList()
            };
        }
    }
}
