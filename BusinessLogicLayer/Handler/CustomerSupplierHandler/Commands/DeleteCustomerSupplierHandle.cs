using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
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
        var entity = _repository.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);
        if (entity == null)
        {
            return new DeleteCustomerSupplierHandleResponse
            {
                Error = true,
                Message = "Müşteri/Tedarikçi bulunamadı."
            };
        }

        // Soft delete: pasif işaretle, silme
        entity.Status = DatabaseAccessLayer.Enumerations.Status.Passive;
        _repository.Update(entity);

        return new DeleteCustomerSupplierHandleResponse
        {
            Error = false,
            Message = "Müşteri/Tedarikçi pasif yapıldı."
        };
    }
}