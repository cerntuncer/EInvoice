using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.InvoiceHandler;
using BusinessLogicLayer.Handler.LineOfInvoiceHandler;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateInvoiceHandle : IRequestHandler<CreateInvoiceHandleRequest, CreateInvoiceHandleResponse>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICurrentRepository _currentRepository;
    private readonly ICustomerSupplierRepository _customerSupplierRepository;
    private readonly IMediator _mediator;
    private readonly MyContext _context;

    public CreateInvoiceHandle(
        IInvoiceRepository invoiceRepository,
        ICurrentRepository currentRepository,
        ICustomerSupplierRepository customerSupplierRepository,
        IMediator mediator,
        MyContext context)
    {
        _invoiceRepository = invoiceRepository;
        _currentRepository = currentRepository;
        _customerSupplierRepository = customerSupplierRepository;
        _mediator = mediator;
        _context = context;
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
        else if (_currentRepository.Find(request.CurrentId) == null || _currentRepository.Find(request.CurrentId).Status != Status.Active)
            message = "Aktif bir kasa/banka bulunamadı.";

        else if (_customerSupplierRepository.Find(request.CustomerSupplierId) == null || _customerSupplierRepository.Find(request.CustomerSupplierId).Status != Status.Active)
            message = "Aktif bir müşteri/tedarikçi bulunamadı.";
        if (message != null)
        {
            return new CreateInvoiceHandleResponse
            {
                Message = message,
                Error = true
            };
        }
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
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

            _invoiceRepository.Add(invoice); // ekleme işlemi yine repo üzerinden

            if (request.lineOfInovices != null && request.lineOfInovices.Count > 0)
            {
                foreach (var line in request.lineOfInovices)
                {
                    var newlineInvoice = new CreateLineOfInvoiceHandleRequest
                    {
                        InvoiceId = invoice.Id,
                        ProductAndServiceId = line.ProductAndServiceId,
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice,
                    };
                    var newLine = await _mediator.Send(newlineInvoice, cancellationToken);
                    if (newLine.Error)
                    {
                        throw new Exception(newLine.Message);
                    }
                }
            }
            await transaction.CommitAsync();
            return new CreateInvoiceHandleResponse
            {
                Message = "Fatura başarıyla oluşturuldu.",
                Error = false
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new CreateInvoiceHandleResponse
            {
                Message = ex.Message,
                Error = true
            };
        }
     
    }
}
