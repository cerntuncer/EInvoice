using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.CustomerSupplierHandler.DTOs;
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
        string? message = null;

        var entity = _repository.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);
        if (entity == null)
        {
            message = "Müşteri/Tedarikçi bulunamadı.";
        }
        else
        {
            if (!Enum.IsDefined(typeof(CustomerOrSupplierType), request.Type))
                message = "Geçersiz müşteri/tedarikçi tipi.";

            if (message == null && !Enum.IsDefined(typeof(Status), request.Status))
                message = "Geçersiz durum tipi.";
        }

        if (message != null)
        {
            return new UpdateCustomerSupplierHandleResponse
            {
                Error = true,
                Message = message
            };
        }

        entity!.Type = request.Type;
        entity.Status = request.Status;

        _repository.Update(entity);

        return new UpdateCustomerSupplierHandleResponse
        {
            Error = false,
            Message = "Müşteri/Tedarikçi güncellendi."
        };
    }
}