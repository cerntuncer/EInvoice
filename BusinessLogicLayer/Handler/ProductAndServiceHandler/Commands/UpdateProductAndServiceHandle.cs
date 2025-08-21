// UpdateProductAndServiceHandle.cs
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdateProductAndServiceHandle : IRequestHandler<UpdateProductAndServiceHandleRequest, UpdateProductAndServiceHandleResponse>
{
    private readonly IProductAndServiceRepository _repository;

    public UpdateProductAndServiceHandle(IProductAndServiceRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateProductAndServiceHandleResponse> Handle(UpdateProductAndServiceHandleRequest request, CancellationToken cancellationToken)
    {
        var entity = _repository.Find(request.Id);
        if (entity == null || entity.Status == Status.Passive)
        {
            return new UpdateProductAndServiceHandleResponse
            {
                Error = true,
                Message = "Ürün/Hizmet bulunamadı veya pasif durumda."
            };
        }

        if (entity.UserId != request.UserId)
        {
            return new UpdateProductAndServiceHandleResponse
            {
                Error = true,
                Message = "Bu kaydı güncelleme yetkiniz yok."
            };
        }

        entity.Name = request.Name;
        entity.price = request.UnitPrice;
        entity.UnitType = request.UnitType;

        _repository.Update(entity);

        return new UpdateProductAndServiceHandleResponse
        {
            Error = false,
            Message = "Ürün/Hizmet güncellendi."
        };
    }
}