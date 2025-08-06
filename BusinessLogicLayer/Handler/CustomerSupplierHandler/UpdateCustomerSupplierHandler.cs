using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CustomerSupplierHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdateCustomerSupplierHandle : IRequestHandler<UpdateCustomerSupplierHandleRequest, UpdateCustomerSupplierHandleResponse>
{
    private readonly ICustomerSupplierRepository _repository;

    public UpdateCustomerSupplierHandle(ICustomerSupplierRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateCustomerSupplierHandleResponse> Handle(UpdateCustomerSupplierHandleRequest request, CancellationToken cancellationToken)
    {
        var entity = _repository.Find(request.Id);
        if (entity == null)
        {
            return new UpdateCustomerSupplierHandleResponse
            {
                Error = true,
                Message = "Müşteri/Tedarikçi bulunamadı."
            };
        }

        if (!Enum.IsDefined(typeof(CustomerOrSupplierType), request.Type))
        {
            return new UpdateCustomerSupplierHandleResponse
            {
                Error = true,
                Message = "Geçersiz müşteri/tedarikçi tipi."
            };
        }

        if (!Enum.IsDefined(typeof(Status), request.Status))
        {
            return new UpdateCustomerSupplierHandleResponse
            {
                Error = true,
                Message = "Geçersiz durum tipi."
            };
        }

        entity.Type = request.Type;
        entity.Status = request.Status;

        _repository.Update(entity);

        return new UpdateCustomerSupplierHandleResponse
        {
            Error = false,
            Message = "Müşteri/Tedarikçi güncellendi."
        };
    }
}
