using BusinessLogicLayer.Handler.InvoiceHandler.DTOs;
using DatabaseAccessLayer.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Handler.InvoiceHandler.Queries
{
    public class GetInvoicesByUserIdHandle : IRequestHandler<GetInvoicesByUserIdHandleRequest, GetInvoicesByUserIdHandleResponse>
    {
        private readonly MyContext _context;

        public GetInvoicesByUserIdHandle(MyContext context)
        {
            _context = context;
        }

        public async Task<GetInvoicesByUserIdHandleResponse> Handle(GetInvoicesByUserIdHandleRequest request, CancellationToken cancellationToken)
        {
            if (request.UserId <= 0)
            {
                return new GetInvoicesByUserIdHandleResponse
                {
                    Error = true,
                    Message = "Geçersiz kullanıcı."
                };
            }

            var invoices = await _context.Invoices
                .Include(i => i.LineOfInvoices)
                .Include(i => i.Current)
                .Where(i => i.Current.UserId == request.UserId)
                .OrderByDescending(i => i.Date)
                .ToListAsync(cancellationToken);

            var items = invoices.Select(inv => new InvoiceSummaryDto
            {
                Id = inv.Id,
                Type = inv.Type,
                Senario = inv.Senario,
                Date = inv.Date,
                No = inv.No,
                CurrentId = inv.CurrentId,
                CustomerSupplierId = inv.CustomerSupplierId,
                Status = inv.Status,
                LineCount = inv.LineOfInvoices.Count,
                TotalAmount = inv.LineOfInvoices.Sum(l => l.UnitPrice * l.Quantity)
            }).ToList();

            return new GetInvoicesByUserIdHandleResponse
            {
                Error = false,
                Message = "Faturalar getirildi.",
                Items = items
            };
        }
    }
}