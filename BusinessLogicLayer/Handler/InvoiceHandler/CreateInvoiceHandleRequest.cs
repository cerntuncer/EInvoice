﻿using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.InvoiceHandler
{
    public class CreateInvoiceHandleRequest : IRequest<CreateInvoiceHandleResponse>
    {
        public InvoiceType Type { get; set; }
        public InvoiceSenario Senario { get; set; }
        public long CurrentId { get; set; }
        public long CustomerSupplierId { get; set; }
        public Status Status { get; set; }
    }
}
