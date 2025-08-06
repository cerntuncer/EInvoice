using MediatR;

namespace BusinessLogicLayer.Handler.LineOfInvoiceHandler
{
    public class UpdateLineOfInvoiceHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
    }
}
