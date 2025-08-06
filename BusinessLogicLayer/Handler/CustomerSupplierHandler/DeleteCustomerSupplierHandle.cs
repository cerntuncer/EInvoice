using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CustomerSupplierHandler;
using MediatR;

public class DeleteCustomerSupplierHandle : IRequestHandler<DeleteCustomerSupplierHandleRequest, DeleteCustomerSupplierHandleResponse>
{
    private readonly ICustomerSupplierRepository _repository;

    public DeleteCustomerSupplierHandle(ICustomerSupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeleteCustomerSupplierHandleResponse> Handle(DeleteCustomerSupplierHandleRequest request, CancellationToken cancellationToken)
    {
        var entity = _repository.Find(request.Id);
        if (entity == null)
        {
            return new DeleteCustomerSupplierHandleResponse
            {
                Error = true,
                Message = "Müşteri/Tedarikçi bulunamadı."
            };
        }

        _repository.Delete(entity);

        return new DeleteCustomerSupplierHandleResponse
        {
            Error = false,
            Message = "Müşteri/Tedarikçi silindi."
        };
    }
}
