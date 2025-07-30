using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.InvoiceHandler;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateInvoiceHandle : IRequestHandler<CreateInvoiceHandleRequest, CreateInvoiceHandleResponse>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICurrentRepository _currentRepository;
    private readonly ICustomerSupplierRepository _customerSupplierRepository;

    public CreateInvoiceHandle(
        IInvoiceRepository invoiceRepository,
        ICurrentRepository currentRepository,
        ICustomerSupplierRepository customerSupplierRepository)
    {
        _invoiceRepository = invoiceRepository;
        _currentRepository = currentRepository;
        _customerSupplierRepository = customerSupplierRepository;
    }

    public async Task<CreateInvoiceHandleResponse> Handle(CreateInvoiceHandleRequest request, CancellationToken cancellationToken)
    {
        string message = null;

        if (request == null)
            message = "Request boş olamaz.";
        else if (!Enum.IsDefined(typeof(InvoiceType), request.Type))
            message = "Fatura tipi geçersiz.";
        else if (!Enum.IsDefined(typeof(InvoiceSenario), request.Senario))
            message = "Fatura senaryosu geçersiz.";

        var current = _currentRepository.Find(request.CurrentId);
        if (current == null || current.Status != Status.Active)
            message = "Aktif bir kasa/banka bulunamadı.";

        var customerSupplier = _customerSupplierRepository.Find(request.CustomerSupplierId);
        if (customerSupplier == null || customerSupplier.Status != Status.Active)
            message = "Aktif bir müşteri/tedarikçi bulunamadı.";

        if (message != null)
        {
            return new CreateInvoiceHandleResponse
            {
                Message = message,
                Error = true
            };
        }

        var invoice = new Invoice
        {
            Type = request.Type,
            Senario = request.Senario,
            CurrentId = request.CurrentId,
            CustomerSupplierId = request.CustomerSupplierId,
            Status = request.Status,
            Date = DateTime.UtcNow,
            No = new Random().Next(1000, 9999)
        };

        _invoiceRepository.Add(invoice);

        return new CreateInvoiceHandleResponse
        {
            Message = "Fatura başarıyla oluşturuldu.",
            Error = false
        };
    }
}
